using DataAccess.Models;
using Microsoft.Bot.Connector;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Services
{
	[Serializable]
	public class NotificationJob : IJob
	{
		private User _user;
		private string _description;

		public async void Execute(IJobExecutionContext context)
		{
			JobKey key = context.JobDetail.Key;

			JobDataMap dataMap = context.MergedJobDataMap;

			_description = dataMap.GetString("description");

			_user = (User)dataMap["user"];
			await ResumeConversation();
		}

		private async Task ResumeConversation()
		{
			var userAccount = new ChannelAccount(_user.ToId, _user.ToName);
			var botAccount = new ChannelAccount(_user.FromId, _user.FromName);
			var connector = new ConnectorClient(new Uri(_user.ServiceUrl));

			IMessageActivity message = Activity.CreateMessageActivity();
			message.ChannelId = _user.ChannelId;
			message.From = botAccount;
			message.Recipient = userAccount;
			message.Conversation = new ConversationAccount(id: _user.ConversationId);
			message.Text = $"Вы просили известить вас в это время. Описание: {_description}";
			message.Locale = "en-Us";
			await connector.Conversations.SendToConversationAsync((Activity)message);
		}
	}
}