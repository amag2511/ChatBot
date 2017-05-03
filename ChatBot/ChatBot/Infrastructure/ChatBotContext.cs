using ChatBot.Migrations;
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
			//Database.Initialize(true);
		}

		public DbSet<DataAccess.Models.Message> Message { get; set; }
		public DbSet<DataAccess.Models.User> User { get; set; }
		public DbSet<DataAccess.Models.Attachment> Attachment { get; set; }
		public DbSet<DataAccess.Models.Help> Help { get; set; }
	}
}
