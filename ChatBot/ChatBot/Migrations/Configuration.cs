namespace ChatBot.Migrations
{
	using DataAccess.Models;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.ChatBotContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.ChatBotContext context)
        {
			context.User.AddOrUpdate(x => x.Id,
					new User() { Id = "dsadsa", Name = "Jane Austen" },
					new User() { Id = "12321dsa", Name = "Charles Dickens" },
					new User() { Id = "321ddsadsa", Name = "Miguel de Cervantes" }
					);
			context.Message.AddOrUpdate(x => x.Id,
					new Message() { Id = "dsadsa", Tag = "SprintPlans", BotsMessage = "продолжаем фиксать баги, которые мы должны закрыть в этом спринте" },
					new Message() { Id = "12321dsa", Tag = "PlansToday", BotsMessage = "Начало рабочего дня начинается в 9 00 и заканчивается в 18 00. Запланированное ежедневное совещание с командой. Встреча с заказчиком" }
					);
			context.Attachment.AddOrUpdate(x => x.Id,
					new Attachment() { Id = "dsadsa", Tag = "jira", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = "12321dsa", Tag = "bitbucket", Description = "ссылка на репозиторий нашей команды", UriAttachment = "https://bitbucket.org" },
					new Attachment() { Id = "321ddsadsa", Tag = "jira123", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = "321ddsadsadsadsa", Tag = "123jira123", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" }
					);
		}
    }
}
