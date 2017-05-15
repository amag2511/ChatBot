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

		private const string FlightsOption = "Flights";

		private const string HotelsOption = "Hotels";

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
			PromptDialog.Choice(context, OnOptionSelected, new List<string>() { FlightsOption, HotelsOption }, "Are you looking for a flight or a hotel?", "Not a valid option", 3);
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{
				string optionSelected = await result;

				switch (optionSelected)
				{
					case FlightsOption:
						context.Call(new MessageDialog(), this.ResumeAfterOptionDialog);
						break;

					case HotelsOption:
						context.Call(new AttachmentsDialog(), this.ResumeAfterOptionDialog);
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