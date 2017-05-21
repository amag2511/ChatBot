using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBot.Dialogs.Forms
{
	[Serializable]
	public class NotificationForm
	{
		[Prompt("Отлично, введите дату уведомления(пример: 05/01/2009 14:57)")]
		public string Date { get; set; }

		[Prompt("Теперь введите описание!")]
		public string Description { get; set; }
	}
}