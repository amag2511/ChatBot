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
//using Eliza;

namespace ChatBot
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{

        private ChatBotRepository<Help> db = new ChatBotRepository<Help>(new ChatBotContext());

		private string missCommunication = "I dont understand your command";

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
				//await Conversation.SendAsync(message, () => new WelcomeDialog());
				// Handle conversation state changes, like members being added and removed
				// Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
				// Not available in all channels
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		//		ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
		//		// calculate something for us to return
		//		string rep = "I dont understand your command";
		//		/////////////
		//		Activity reply = activity.CreateReply();

		//		reply.Attachments = reply.Attachments ?? new List<Attachment>();

		//				reply.Attachments.Add(activity.Attachments.FirstOrDefault());

		//				await connector.Conversations.ReplyToActivityAsync(reply);
		//				//////////////
		//				return Request.CreateResponse(HttpStatusCode.OK);

		//				if (activity.Text.StartsWith("@"))
		//				{
		//					rep = new string(activity.Text.Skip(1).ToArray());

		//					var message = db.Message?.FirstOrDefault(x => x.Tag == rep);
		//					if(message != null)
		//					{
		//						reply = activity.CreateReply(message.BotsMessage);
		//						await connector.Conversations.ReplyToActivityAsync(reply);
		//	}

		//	var attachments = db.Attachment.FirstOrDefault(x => x.Tag == rep);
		//					if (attachments != null)
		//					{
		//						reply = activity.CreateReply($"{attachments.Description}\n{attachments.UriAttachment}");
		//						await connector.Conversations.ReplyToActivityAsync(reply);
		//}
		//					else if (message == null && attachments == null)
		//					{
		//						reply = activity.CreateReply(missCommunication);
		//						await connector.Conversations.ReplyToActivityAsync(reply);
		//					}
	}
}