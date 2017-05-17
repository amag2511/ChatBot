using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using DataAccess.Models;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class SearchDialog : IDialog<object>
	{
		private User _user;

		public SearchDialog()
		{
		}

		public SearchDialog(User user)
		{
			this._user = user;
		}

		public Task StartAsync(IDialogContext context)
		{
			throw new NotImplementedException();
		}
	}
}