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
					new Attachment() { Id = 3, Tag = "jira123", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = 4, Tag = "123jira123", Description = "ссылка на jira нашей команды", UriAttachment = "https://orionjira.atlassian.net" }
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
