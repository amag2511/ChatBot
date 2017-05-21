using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ChatBot.Dialogs.Queries;
using System.Web;
using System.IO;
using System.Collections.Generic;
using DataAccess.Models;
using ChatBot.Repository;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class ReceiveAttachmentDialog : IDialog<object>
	{
		private User _user;
		private AddAttachmentForm _state;

		public ReceiveAttachmentDialog(User user, AddAttachmentForm state)
		{
			_user = user;
			_state = state;
		}

		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;

			try
			{
				await SaveAttachmentToRepository(message);

				await context.PostAsync("Сохранение прошло успешно");
			}
			catch
			{
				context.Fail(new ArgumentException("Что-то пошло не так, убедитесь, что вы отправили именно медиа элемент"));
			}
			finally
			{
				context.Done<object>(null);
			}
		}

		private async Task SaveAttachmentToRepository(IMessageActivity message)
		{
			var attachment = message.Attachments.First();

			using (HttpClient httpClient = new HttpClient())
			{
				if ((message.ChannelId.Equals("skype", StringComparison.InvariantCultureIgnoreCase) || message.ChannelId.Equals("msteams", StringComparison.InvariantCultureIgnoreCase))
					&& new Uri(attachment.ContentUrl).Host.EndsWith("skype.com"))
				{
					var token = await new MicrosoftAppCredentials().GetTokenAsync();
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}

				var responseMessage = await httpClient.GetAsync(attachment.ContentUrl);
				var attachmentsInBytes = await responseMessage.Content.ReadAsByteArrayAsync();

				var imageData = Convert.ToBase64String(attachmentsInBytes);

				var mediaElement = new MediaElement
				{
					ContentData = await responseMessage.Content.ReadAsByteArrayAsync(),
					ContentType = attachment.ContentType,
					Tag = _state.Tag,
					Name = attachment.Name,
					Description = _state.Description
				};

				using (var repository = new ChatBotRepository<MediaElement>())
				{
					mediaElement.UserId = _user.Id;

					repository.Create(mediaElement);
				}
			}
		}
	}
}