using ChatBot.Migrations;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatBot.Infrastructure
{
    public class ChatBotContext : DbContext
    {
		// You can add custom code to this file. Changes will not be overwritten.
		// 
		// If you want Entity Framework to drop and regenerate your database
		// automatically whenever you change your model schema, please use data migrations.
		// For more information refer to the documentation:
		// http://msdn.microsoft.com/en-us/data/jj591621.aspx

		public ChatBotContext() : base("name=ChatBotContext")
        {
		}

		public DbSet<Message> Message { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Attachment> Attachment { get; set; }
		public DbSet<Help> Help { get; set; }
		public DbSet<Notification> Notification { get; set; }
	}
}
