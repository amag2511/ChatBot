using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Threading;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{

		private const string AttachmentsOption = "Прикрепления";

		private const string NotificationsOption = "Уведомления";

		private const string SearchOption = "Поиск";

		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			var message = await result;

			if (message.Text.ToLower().Contains("help"))
			{
				await context.Forward(new HelpDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
			}
			else
			{
				ShowOptions(context);
			}
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context, OnOptionSelected, new List<string>() { NotificationsOption, AttachmentsOption, SearchOption }, "Какое действие желаете выполнить?");
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{
				string optionSelected = await result;

				switch (optionSelected)
				{
					case NotificationsOption:
						context.Call(new MessageDialog(), this.ResumeAfterOptionDialog);
						break;
					case AttachmentsOption:
						context.Call(new AttachmentsDialog(), this.ResumeAfterOptionDialog);
						break;
					case SearchOption:
						context.Call(new SearchDialog(), this.ResumeAfterOptionDialog);
						break;

				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

				context.Wait(this.MessageReceivedAsync);
			}
		}

		private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
		{
			var ticketNumber = await result;

			await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
			context.Wait(this.MessageReceivedAsync);
		}

		private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
		{
			try
			{
				var message = await result;
			}
			catch (Exception ex)
			{
				await context.PostAsync($"Failed with message: {ex.Message}");
			}
			finally
			{
				context.Wait(this.MessageReceivedAsync);
			}
		}
	}
}