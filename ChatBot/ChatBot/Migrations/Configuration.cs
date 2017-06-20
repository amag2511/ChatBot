namespace ChatBot.Migrations
{
	using DataAccess.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.ChatBotContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
			CommandTimeout = 30;
        }

        protected override void Seed(Infrastructure.ChatBotContext context)
        {
			context.Help.AddOrUpdate(x => x.Id,
					new Help() { Id = 1, Command = "commands", Description = "Открывает диалог с доступными коммандами. Для вызова отправьте сообщение, содержащее ключевое слово commands." }
					);
			context.Notification.AddOrUpdate(x => x.Id,
					new Notification() { Id = 1, Date = DateTime.Now , Description = "Для вызова notification" }
					);
		}
    }
}
