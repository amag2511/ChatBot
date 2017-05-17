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
					new Help() { Id = 1, Command = "Add-notification", Description = "��� ����, ����� �������� �����������, ���������� ��������: Add-notification [current date] [description]" },
					new Help() { Id = 1, Command = "Add-attachment", Description = "��� ����, ����� �������� ������������, ���������� ��������: Add-attachment [media element]" },
					new Help() { Id = 2, Command = "Delete-notificaton", Description = "������� �����������. Delete-notificaton [title]" },
					new Help() { Id = 2, Command = "Delete-attachment", Description = "������� ������������. Delete-attachment [name]" },
					new Help() { Id = 2, Command = "Search-by-name", Description = "������ �������� ��������� �������� � ������� ����� ������� �� �����. Search-by-name [Type] [Name], ��� Type - ��� �������� ��������, Name - ��� ��������" },
					new Help() { Id = 3, Command = "Search-by-tag", Description = "������ �������� ��������� �������� � ������� ����� �������� �� ����. Search-by-tag [Type] [Name], ��� Type - ��� �������� ��������, Name - ��� ��������" }
					);
		}
    }
}
