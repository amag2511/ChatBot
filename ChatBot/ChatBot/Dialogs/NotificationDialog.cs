using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataAccess.Models;
using Microsoft.Bot.Builder.Dialogs;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class NotificationDialog : IDialog<object>
	{
		private User _user;

		public NotificationDialog()
		{
		}

		public NotificationDialog(User user)
		{
			this._user = user;
		}

		public async Task StartAsync(IDialogContext context)
		{
			context.Fail(new NotImplementedException("Flights Dialog is not implemented and is instead being used to show context.Fail"));
		}
	}
}