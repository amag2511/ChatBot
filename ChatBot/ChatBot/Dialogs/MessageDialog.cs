using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class MessageDialog : IDialog<object>
	{
		public async Task StartAsync(IDialogContext context)
		{
			context.Fail(new NotImplementedException("Flights Dialog is not implemented and is instead being used to show context.Fail"));
		}
	}
}