using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using ChatBot.Infrastructure;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using ChatBot.Dialogs;
using ChatBot.Repository;
using DataAccess.Models;
using ChatBot.Resources;
using System.Collections.ObjectModel;
//using Eliza;

namespace ChatBot
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{
		private ChatBotRepository<User> _repository = new ChatBotRepository<User>();

		/// <summary>
		/// POST: api/Messages
		/// Receive a message from a user and reply to it
		/// </summary>
		public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				await Conversation.SendAsync(activity, () => new RootDialog());
			}
			else
			{
				await HandleSystemMessage(activity);
			}
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}


		private async Task<Activity> HandleSystemMessage(Activity message)
		{
			if (message.Type == ActivityTypes.DeleteUserData)
			{
				// Implement user deletion here
				// If we handle user deletion, return a real message
			}

			else if (message.Type == ActivityTypes.ConversationUpdate)
			{
				if(message.MembersAdded != null && message.MembersAdded.Any() && !(message.MembersAdded.Any(x => x.Name == "Bot") && message.MembersAdded.Count == 1))
				{
					await GenerateWelcomeMessage(message);

					List <User> members = new List<User>();
					foreach(var user in message.MembersAdded)
					{
						_repository.Create(new User
						{
							ConversationId = message.Conversation.Id,
							Name = user.Name
						});
					}
				}

				if (message.MembersRemoved != null && message.MembersRemoved.Any())
				{
					var members = _repository.GetCollection.Where(x => x.ConversationId == message.Conversation.Id)
						.Where(x => message.MembersRemoved.FirstOrDefault(y => y.Name == x.Name) != null);
					foreach(var user in members)
					{
						_repository.Remove(user);
					}
				}
			}
			else if (message.Type == ActivityTypes.ContactRelationUpdate)
			{

				// Handle add/remove from contact lists
				// Activity.From + Activity.Action represent what happened
				//await Conversation.SendAsync(message, () => new WelcomeDialog());
			}
			else if (message.Type == ActivityTypes.Typing)
			{
				// Handle knowing tha the user is typing
			}
			else if (message.Type == ActivityTypes.Ping)
			{
			}

			return null;
		}

		private async Task GenerateWelcomeMessage(Activity message)
		{
			var connector = new ConnectorClient(new Uri(message.ServiceUrl));

			var reply = message.CreateReply(ChatBotResources.WELCOME_MESSAGE);

			await connector.Conversations.ReplyToActivityAsync(reply);
		}
	}
}