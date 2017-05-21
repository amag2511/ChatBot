using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataAccess.Models;
using Microsoft.Bot.Builder.Dialogs;
using ChatBot.Dialogs.Forms;
using Microsoft.Bot.Builder.FormFlow;
using ChatBot.Repository;
using ChatBot.Services;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class NotificationDialog : IDialog<object>
	{
		private User _user;
		private GlobalData data = GlobalData.GetInstance();

		public NotificationDialog(User user)
		{
			_user = user;
		}

		public async Task StartAsync(IDialogContext context)
		{
			var AddAttachmentFormDialog = FormDialog.FromForm(BuildAddAttachmentForm, FormOptions.PromptInStart);
			context.Call(AddAttachmentFormDialog, ResumeAfterAddFormDialog);
		}

		private async Task ResumeAfterAddFormDialog(IDialogContext context, IAwaitable<NotificationForm> result)
		{
			try
			{
				AddNotificationDetailsToDatabase(await result);
				await context.PostAsync($"Уведомление добавлено успешно!");
			}
			catch (FormCanceledException ex)
			{
				string reply;

				if (ex.InnerException == null)
				{
					reply = "Операция по добавлению была отменена";
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

		private IForm<NotificationForm> BuildAddAttachmentForm()
		{
			OnCompletionAsyncDelegate<NotificationForm> processAddNotification = async (context, state) =>
			{
				await context.PostAsync($"Хорошо. Уведомление будет добавлено на эту дату!");
			};

			return new FormBuilder<NotificationForm>()
				.AddRemainingFields()
				.OnCompletion(processAddNotification)
				.Build();
		}

		private void AddNotificationDetailsToDatabase(NotificationForm state)
		{
			DateTime date;
			DateTime.TryParse(state.Date, out date);

			using (var repository = new ChatBotRepository<Notification>())
			{
				var notification = new Notification
				{
					Date = date,
					Description = state.Description,
					UserId = _user.Id
				};

				repository.Create(notification);

				notification.User = _user;
				data.NotifyService.RunScheduler(notification);
			}
		}
	}
}