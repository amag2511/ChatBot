using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;

namespace ChatBot.Dialogs.Queries
{
	[Serializable]
	public class AddAttachmentForm
	{
		[Prompt("Введите уникальное имя для файла")]
		public string Name { get; set; }

		[Prompt("Хорошо, введите тег для файла")]
		public string Tag { get; set; }

		[Prompt("Теперь загрузите файл")]
		public Activity MediaElement { get; set; }
	}
}