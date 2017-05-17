using DataAccess.Models;
using System;
using System.Data.Entity;

namespace ChatBot.Infrastructure
{
	[Serializable]
    public class ChatBotContext : DbContext
    {
		public ChatBotContext() : base("name=ChatBotContext")
        {
		}

		public DbSet<User> User { get; set; }
		public DbSet<MediaElement> Attachment { get; set; }
		public DbSet<Help> Help { get; set; }
		public DbSet<Notification> Notification { get; set; }
	}
}
