using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;

namespace ChatBot.Dialogs.Queries
{
	[Serializable]
	public class AddAttachmentForm
	{
		[Prompt("Введите описание к файлу")]
		public string Description { get; set; }

		[Prompt("Хорошо, введите тег для файла")]
		public string Tag { get; set; }
	}
}