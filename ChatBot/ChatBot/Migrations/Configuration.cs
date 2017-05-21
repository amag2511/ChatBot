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
        }

        protected override void Seed(Infrastructure.ChatBotContext context)
        {
			context.Help.AddOrUpdate(x => x.Id,
					new Help() { Id = 1, Command = "command", Description = "Для вызова меня с доступными коммандами напишите command" }
					);
		}
    }
}
