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

		private readonly ChatBotRepository<User> _repository;
		private readonly IEnumerable<DataAccess.Models.Attachment> _attachments;

		//public AttachmentsDialog()
		//{
		//	using (var rep = new ChatBotRepository<User>(new ChatBotContext()))
		//	{
		//		_attachments = rep.FindById(1).Attachments.ToList();
		//	}

		//}

		public async Task StartAsync(IDialogContext context)
		{
			await context.PostAsync("Nice");

			context.Wait(MessageReceivedAsync);
			//ShowOptions(context);
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context, OnOptionSelected, new List<string>() { ADD_ATTACHMENT, DELETE_ATTACHMENT }, "Какое действие желаете выполнить?", "Attachments crashed promt", 3);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			ShowOptions(context);
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
						var allAttachments = string.Format(ChatBotResources.DELETE_ATTACHMENT, string.Join(", ", _attachments?.Select(x => x.Name)));
						await context.PostAsync(allAttachments);
						var DeleteAttachmentFormDialog = FormDialog.FromForm(this.BuildDeleteAttachmentForm, FormOptions.PromptInStart);
						context.Call(DeleteAttachmentFormDialog, this.ResumeAfterDeleteFormDialog);
						break;
				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Упс, слишком много попыток. Однако ты можешь попытаться попробовать снова!");

				context.Wait(this.MessageReceivedAsync);
			}
		}


		private IForm<AddAttachmentForm> BuildAddAttachmentForm()
		{
			OnCompletionAsyncDelegate<AddAttachmentForm> processHotelsSearch = async (context, state) =>
			{
				await context.PostAsync($"Хорошо. Файл с тегом {state.Tag} будет загружен на сервер...");
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
				
			}
			catch (FormCanceledException ex)
			{
				string reply;

				if (ex.InnerException == null)
				{
					reply = "You have canceled the operation. Quitting from the HotelsDialog";
				}
				else
				{
					reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
				}

				await context.PostAsync(reply);
			}
			finally
			{
				context.Done<object>(null);
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
	}
}