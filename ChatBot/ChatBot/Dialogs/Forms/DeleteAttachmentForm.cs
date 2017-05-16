using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBot.Dialogs.Queries
{
	[Serializable]
	public class DeleteAttachmentForm
	{
		[Prompt("Введите имя для файла, которое хотите удалить")]
		public string Name { get; set; }
	}
}