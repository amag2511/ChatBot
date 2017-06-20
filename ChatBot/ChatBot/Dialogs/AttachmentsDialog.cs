using ChatBot.Dialogs.Queries;
using ChatBot.Infrastructure;
using ChatBot.Repository;
using ChatBot.Resources;
using DataAccess.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class AttachmentsDialog : IDialog<object>
	{
		private const string AddAttachmentOption = "Добавить";
		private const string DeleteAttachmentOption = "Удалить";
		private const string CancelOption = "Отмена";
		private User _user;

		public AttachmentsDialog(User user)
		{
			_user = user;
		}

		public async Task StartAsync(IDialogContext context)
		{
			ShowOptions(context);
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context,
								OnOptionSelected,
								new List<string>() { AddAttachmentOption, DeleteAttachmentOption, CancelOption },
								"Какое действие желаете выполнить с вложениями?",
								"Недопустимая операция, попробуйте снова.",
								3);
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{ 
				string optionSelected = await result;

				switch (optionSelected)
				{
					case AddAttachmentOption:
						var AddAttachmentFormDialog = FormDialog.FromForm(BuildAddAttachmentForm, FormOptions.PromptInStart);
						context.Call(AddAttachmentFormDialog, ResumeAfterAddFormDialog);
						break;
					case DeleteAttachmentOption:
						var allAttachments = string.Format(ChatBotResources.DELETE_ATTACHMENT, string.Join(", ", GetMediaElementsNames()));
						await context.PostAsync(allAttachments);
						var DeleteAttachmentFormDialog = FormDialog.FromForm(BuildDeleteAttachmentForm, FormOptions.PromptInStart);
						context.Call(DeleteAttachmentFormDialog, ResumeAfterDeleteFormDialog);
						break;
					case CancelOption:
						await context.PostAsync($"Операция была отменена!");
						context.Done<object>(null);
						break;
				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Упс, слишком много попыток!");
				context.Done<object>(null);
			}
		}

		private IForm<AddAttachmentForm> BuildAddAttachmentForm()
		{
			OnCompletionAsyncDelegate<AddAttachmentForm> processHotelsSearch = async (context, state) =>
			{
				await context.PostAsync($"Хорошо. Теперь загрузите файл");
			};

			return new FormBuilder<AddAttachmentForm>()
				.AddRemainingFields()
				.OnCompletion(processHotelsSearch)
				.Build();
		}

		private IForm<DeleteAttachmentForm> BuildDeleteAttachmentForm()
		{
			OnCompletionAsyncDelegate<DeleteAttachmentForm> processHotelsSearch = async (context, state) =>
			{
				await context.PostAsync($"Хорошо. Файл {state.Name} будет удален с сервера...");
			};

			return new FormBuilder<DeleteAttachmentForm>()
				.AddRemainingFields()
				.OnCompletion(processHotelsSearch)
				.Build();
		}

		private async Task ResumeAfterAddFormDialog(IDialogContext context, IAwaitable<AddAttachmentForm> result)
		{
			try
			{
				context.Call(new ReceiveAttachmentDialog(_user, await result), AddMessageRecieve);
			}
			catch
			{
				await context.PostAsync(ChatBotResources.DEFAULT_EXCEPTION);
			}
		}

		private async Task AddMessageRecieve(IDialogContext context, IAwaitable<object> result)
		{
			context.Done<object>(null);
		}

		private async Task ResumeAfterDeleteFormDialog(IDialogContext context, IAwaitable<DeleteAttachmentForm> result)
		{
			try
			{
				await context.PostAsync(RemoveMediaElementFromDatabase((await result).Name));
			}
			catch
			{
				string reply = $"Упс, что-то пошло не так";

				await context.PostAsync(reply);
			}
			finally
			{
				context.Done<object>(null);
			}
		}

		private string RemoveMediaElementFromDatabase(string name)
		{
			int? mediaItemId;

			using (var repository = new ChatBotRepository<User>())
			{
				mediaItemId = repository.GetSender(_user.ConversationId, _user.ToName).MediaElements.FirstOrDefault(x => x.Name == name)?.Id;
			}

			using (var repository = new ChatBotRepository<MediaElement>())
			{
				if (mediaItemId == null)
				{
					return "Упс, но такого файла нет в базе";
				}

				var mediaItem = repository.FindById((int)mediaItemId);
				repository.Remove(mediaItem);

				return "Удаление прошо успешно, поздравляю(party)";
			}
		}

		private IEnumerable<string> GetMediaElementsNames()
		{
			using (var repository = new ChatBotRepository<User>())
			{
				return repository.GetSender(_user.ConversationId, _user.ToName).MediaElements.Select(x => x.Name);
			}
		}
	}
}