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
			var AddNotificationForm = FormDialog.FromForm(BuildAddNotificationForm, FormOptions.PromptInStart);
			context.Call(AddNotificationForm, ResumeAfterAddFormDialog);
		}

		private async Task ResumeAfterAddFormDialog(IDialogContext context, IAwaitable<NotificationForm> result)
		{
			try
			{
				AddNotificationDetailsToDatabase(await result);
				await context.PostAsync($"Уведомление добавлено успешно!");
			}
			catch
			{
				string reply = $"Ой! Что-то пошло не так :( Возможно вы ввели некорректную дату.";

				await context.PostAsync(reply);
			}
			finally
			{
				context.Done<object>(null);
			}
		}

		private IForm<NotificationForm> BuildAddNotificationForm()
		{
			OnCompletionAsyncDelegate<NotificationForm> processAddNotification = async (context, state) =>
			{
				await context.PostAsync($"Отлично!! Уведомление будет добавлено на эту дату!");
			};

			return new FormBuilder<NotificationForm>()
				.AddRemainingFields()
				.OnCompletion(processAddNotification)
				.Build();
		}

		private void AddNotificationDetailsToDatabase(NotificationForm state)
		{
			DateTime date = DateTime.Parse(state.Date);
			User user;

			using (var repository = new ChatBotRepository<User>())
			{
				user = repository.GetSender(_user.ConversationId, _user.ToName);
				user.MediaElements = null;
				user.Notifications = null;
			}

			using (var repository = new ChatBotRepository<Notification>())
			{
				var notification = new Notification
				{
					Date = date,
					Description = state.Description,
					UserId = user.Id
				};

				repository.Create(notification);

				notification.User = user;
				RunScheduller(notification);
			}
		}

		private void RunScheduller(Notification notification)
		{
			new NotifyService().RunScheduler(notification);
		}
	}
}