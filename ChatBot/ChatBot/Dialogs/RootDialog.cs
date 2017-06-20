using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Threading;
using ChatBot.Repository;
using DataAccess.Models;
using ChatBot.Services;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{
		//private GlobalData data = GlobalData.GetInstance();

		private const string AttachmentsOption = "Вложения";
		private const string NotificationsOption = "Уведомления";
		private const string SearchOption = "Поиск";
		private const string CancelOption = "Отмена";

		private User _user;
		private NotifyService _notifyService;

		public RootDialog()
		{
			_notifyService = new NotifyService();
		}
		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			var message = await result;

			_user = new User
			{
				ToId = message.From.Id,
				ToName = message.From.Name,
				FromId = message.Recipient.Id,
				FromName = message.Recipient.Name,
				ServiceUrl = message.ServiceUrl,
				ChannelId = message.ChannelId,
				ConversationId = message.Conversation.Id,
			};

			if (message.Text.ToLower().Contains("help"))
			{
				await context.Forward(new HelpDialog(), ResumeAfterSupportDialog, message, CancellationToken.None);
			}
			else if(message.Text.ToLower().Contains("commands"))
			{
				ShowOptions(context);
			}
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context,
								OnOptionSelected,
								new List<string>() { NotificationsOption, AttachmentsOption, SearchOption, CancelOption },
								"С чем хотите поработать?:)",
								"Недопустимая операция, попробуйте снова.",
								3);
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{
				string optionSelected = await result;

				var user = GetCurrentUser(_user);
				user = null;

				switch (optionSelected)
				{
					case NotificationsOption:
						context.Call(new NotificationDialog(_user), ResumeAfterOptionDialog);
						break;
					case AttachmentsOption:
						context.Call(new AttachmentsDialog(_user), ResumeAfterOptionDialog);
						break;
					case SearchOption:
						context.Call(new SearchDialog(_user), ResumeAfterOptionDialog);
						break;
					case CancelOption:
						await context.PostAsync($"Операция была отменена!");
						context.Wait(MessageReceivedAsync);
						break;
				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Упс! слишком много попыток :(. Введите 'help' для помощи!");

				context.Wait(MessageReceivedAsync);
			}
		}

		private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
		{
			context.Wait(MessageReceivedAsync);
		}

		private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
		{
			try
			{
				var message = await result;
			}
			catch (Exception ex)
			{
				await context.PostAsync($"Непредвиденная ошибка: {ex.Message}");
			}
			finally
			{
				context.Wait(MessageReceivedAsync);
			}
		}

		private User GetCurrentUser(User user)
		{
			if (user == null)
			{
				return null;
			}

			User result;

			using (var repository = new ChatBotRepository<User>())
			{
				result = repository.GetSender(user.ConversationId, user.ToName);

				if (result == null)
				{
					repository.Create(user);
					result = user;
				}

				return result;
			}
		}
	}
}