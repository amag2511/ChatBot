using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using ChatBot.Resources;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class WelcomeDialog : IDialog<string>
	{
		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			await context.PostAsync(ChatBotResources.WELCOME_MESSAGE);

			context.Done("done");
		}
	}
}