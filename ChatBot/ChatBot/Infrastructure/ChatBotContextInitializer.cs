using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace ChatBot.Infrastructure
{
	public class ChatBotContextInitializer : DropCreateDatabaseIfModelChanges<ChatBotContext>
	{
		protected override void Seed(ChatBotContext context)
		{
			context.User.AddOrUpdate(x => x.Id,
					new User() { Id = 1, Name = "Jane Austen" },
					new User() { Id = 2, Name = "Charles Dickens" },
					new User() { Id = 3, Name = "Miguel de Cervantes" }
					);
			context.Message.AddOrUpdate(x => x.Id,
					new Message() { Id = 1, Tag = "SprintPlans", BotsMessage = "продолжаем фиксать баги, которые мы должны закрыть в этом спринте" },
					new Message() { Id = 2, Tag = "PlansToday", BotsMessage = "Начало рабочего дня начинается в 9 00 и заканчивается в 18 00. Запланированное ежедневное совещание с командой. Встреча с заказчиком" }
					);
			context.Attachment.AddOrUpdate(x => x.Id,
					new Attachment() { Id = 1, Tag = "jira", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = 2, Tag = "bitbucket", Description = "ссылка на репозиторий нашей команды", UriAttachment = "https://bitbucket.org" },
					new Attachment() { Id = 3, Tag = "jira123", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" }
					
					);
		}
	}
}