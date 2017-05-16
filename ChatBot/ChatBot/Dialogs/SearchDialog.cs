using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class SearchDialog : IDialog<object>
	{
		public Task StartAsync(IDialogContext context)
		{
			throw new NotImplementedException();
		}
	}
}