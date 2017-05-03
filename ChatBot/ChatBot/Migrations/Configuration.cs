namespace ChatBot.Migrations
{
	using DataAccess.Models;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<ChatBot.Infrastructure.ChatBotContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ChatBot.Infrastructure.ChatBotContext context)
        {
			context.User.AddOrUpdate(x => x.Id,
					new User() { Id = 1, Name = "Jane Austen" },
					new User() { Id = 2, Name = "Charles Dickens" },
					new User() { Id = 3, Name = "Miguel de Cervantes" }
					);
			context.Message.AddOrUpdate(x => x.Id,
					new Message() { Id = 1, Tag = "SprintPlans", BotsMessage = "���������� ������� ����, ������� �� ������ ������� � ���� �������" },
					new Message() { Id = 2, Tag = "PlansToday", BotsMessage = "������ �������� ��� ���������� � 9 00 � ������������� � 18 00. ��������������� ���������� ��������� � ��������. ������� � ����������" }
					);
			context.Attachment.AddOrUpdate(x => x.Id,
					new Attachment() { Id = 1, Tag = "jira", Description = "������ �� jira ����� �������", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = 2, Tag = "bitbucket", Description = "������ �� ����������� ����� �������", UriAttachment = "https://bitbucket.org" },
					new Attachment() { Id = 3, Tag = "jira123", Description = "������ �� jira ����� �������", UriAttachment = "https://orionjira.atlassian.net" },
					new Attachment() { Id = 3, Tag = "123jira123", Description = "������ �� jira ����� �������", UriAttachment = "https://orionjira.atlassian.net" }
					);
		}
    }
}
