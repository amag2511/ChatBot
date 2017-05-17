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
			context.User.AddOrUpdate(x => x.Id,
					new User() { Id = 1, Name = "Jane Austen" },
					new User() { Id = 2, Name = "Charles Dickens" },
					new User() { Id = 3, Name = "Miguel de Cervantes" }
					);
			context.Help.AddOrUpdate(x => x.Id,
					new Help() { Id = 1, Command = "Add-notification", Description = "Для того, чтобы добавить уведомление, необходимо написать: Add-notification [current date] [description]" },
					new Help() { Id = 1, Command = "Add-attachment", Description = "Для того, чтобы добавить прикрепление, необходимо написать: Add-attachment [media element]" },
					new Help() { Id = 2, Command = "Delete-notificaton", Description = "Удалить уведомление. Delete-notificaton [title]" },
					new Help() { Id = 2, Command = "Delete-attachment", Description = "Удалить прикрепление. Delete-attachment [name]" },
					new Help() { Id = 2, Command = "Search-by-name", Description = "Данная комманда позволяет получить с сервера медиа элемент по имени. Search-by-name [Type] [Name], Где Type - тип искомого элемента, Name - Имя элемента" },
					new Help() { Id = 3, Command = "Search-by-tag", Description = "Данная комманда позволяет получить с сервера медиа элементы по тэгу. Search-by-tag [Type] [Name], Где Type - тип искомого элемента, Name - Имя элемента" }
					);
		}
    }
}
