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
		private const string ADD_ATTACHMENT = "Добавить";
		private const string DELETE_ATTACHMENT = "Удалить";

		private User _user;

		public AttachmentsDialog(User user)
		{
			this._user = user;
		}

		public AttachmentsDialog()
		{
		}

		public async Task StartAsync(IDialogContext context)
		{
			await context.PostAsync("Nice");

			ShowOptions(context);
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { ADD_ATTACHMENT, DELETE_ATTACHMENT }, "Какое действие желаете выполнить?", "Attachments crashed promt", 3);
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{ 
				string optionSelected = await result;

				switch (optionSelected)
				{
					case ADD_ATTACHMENT:
						var AddAttachmentFormDialog = FormDialog.FromForm(this.BuildAddAttachmentForm, FormOptions.PromptInStart);
						context.Call(AddAttachmentFormDialog, this.ResumeAfterAddFormDialog);
						break;
					case DELETE_ATTACHMENT:
						var allAttachments = string.Format(ChatBotResources.DELETE_ATTACHMENT, string.Join(", ", _user.MediaElements.Select(x => x.Name)));
						await context.PostAsync(allAttachments);
						var DeleteAttachmentFormDialog = FormDialog.FromForm(this.BuildDeleteAttachmentForm, FormOptions.PromptInStart);
						context.Call(DeleteAttachmentFormDialog, this.ResumeAfterDeleteFormDialog);
						break;
				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Упс, слишком много попыток. Однако ты можешь попытаться попробовать снова!");
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
				context.Call(new ReceiveAttachmentDialog(_user, await result), this.ResumeAfterRecieveAttachmentDialog);
			}
			catch (FormCanceledException ex)
			{
				string reply;

				if (ex.InnerException == null)
				{
					reply = "You have canceled the operation. Quitting from the AttachmentDialog";
				}
				else
				{
					reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
				}

				await context.PostAsync(reply);
			}
		}

		private async Task ResumeAfterDeleteFormDialog(IDialogContext context, IAwaitable<DeleteAttachmentForm> result)
		{
			try
			{

			}
			catch (FormCanceledException ex)
			{
				string reply;

				if (ex.InnerException == null)
				{
					reply = "Вы отменили удаление медиа элемента";
				}
				else
				{
					reply = $"Упс, что-то пошло не так. Детали: {ex.InnerException.Message}";
				}

				await context.PostAsync(reply);
			}
			finally
			{
				context.Done<object>(null);
			}
		}
		private async Task ResumeAfterRecieveAttachmentDialog(IDialogContext context, IAwaitable<object> result)
		{
			context.Done<object>(null);
		}
	}
}