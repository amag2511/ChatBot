using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.Bot.Connector;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class SearchDialog : IDialog<object>
	{
		private const string DEFAULT_VIDEO_IMAGE = "http://getinstantvideomachine.com/deluxe-up-demo2/wp-content/uploads/2015/01/video1.png";
		private const string DEFAULT_DOC_IMAGE = "http://www.zamzar.com/images/filetypes/doc.png";
		private const string SEARCH_BY_NAME = "По имени";
		private const string SEARCH_BY_TAG = "по тегам";

		private User _user;

		public SearchDialog()
		{
		}

		public SearchDialog(User user)
		{
			_user = user;
		}

		public async Task StartAsync(IDialogContext context)
		{
			ShowOptions(context);
		}

		private void ShowOptions(IDialogContext context)
		{
			PromptDialog.Choice(context, OnOptionSelected, new List<string>() { SEARCH_BY_TAG, SEARCH_BY_NAME }, "Выберите тип поиска", "Not a valid option", 3);
		}

		private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
		{
			try
			{
				string optionSelected = await result;

				switch (optionSelected)
				{
					case SEARCH_BY_TAG:
						await context.PostAsync("Введите теги, по которым хотите найти медиа элементы");
						context.Wait(MessageReceivedForTagsAsync);
						break;
					case SEARCH_BY_NAME:
						await context.PostAsync("Введите имя искомого медиа элемента");
						context.Wait(MessageReceivedForNameAsync);
						break;
				}
			}
			catch (TooManyAttemptsException ex)
			{
				await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

				context.Wait(MessageReceivedAsync);
			}
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			try
			{
				ShowOptions(context);
			}
			catch
			{
				context.Fail(new ArgumentException("Что-то пошло не так, убедитесь, что все было сделано по инструкции"));
			}
			finally
			{
				context.Done<object>(null);
			}
		}

		public virtual async Task MessageReceivedForTagsAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;

			try
			{
				var elements = SearchAttachmentsByTag(message);
				if (elements == null || !elements.Any())
				{
					await context.PostAsync("К сожалению, ничего не было найдено");
				}
				else
				{
					await ShowCarouselWithMediaElements(context, elements);
				}
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

		public virtual async Task MessageReceivedForNameAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;

			try
			{
				var result = SearchAttachmentsByName(message);

				if(result == null)
				{
					await context.PostAsync("К сожалению, ничего не было найдено");
				}
				else
				{
					var replyMessage = context.MakeMessage();

					replyMessage.Attachments = new List<Attachment> { Map(result) };

					await context.PostAsync(replyMessage);
				}
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

		private async Task ShowCarouselWithMediaElements(IDialogContext context, IEnumerable<MediaElement> elements)
		{
			var reply = context.MakeMessage();

			reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
			reply.Attachments = GetCardsAttachments(elements);

			await context.PostAsync(reply);
		}

		private static IList<Attachment> GetCardsAttachments(IEnumerable<MediaElement> elements)
		{
			var result = new List<Attachment>();
			foreach(var element in elements)
			{
				var type = element.ContentType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

				switch (type)
				{
					case "audio":
						result.Add(GetAudioCard(element));
						break;
					case "video":
						result.Add(GetVideoCard(element));
						break;
					case "image":
						result.Add(GetImageCard(element));
						break;
					default:
						result.Add(GetGeneralCard(element));
						break;

				}
			}
			return result;
		}

		private static Attachment GetImageCard(MediaElement element)
		{
			var heroCard = new HeroCard
			{
				Title = element.Name,
				Subtitle = element.Tag,
				Text = element.Description,
				Images = new List<CardImage>() { new CardImage(url:element.GetContentUrl())}
			};

			return heroCard.ToAttachment();
		}

		private static Attachment GetVideoCard(MediaElement element)
		{
			var videoCard = new VideoCard
			{
				Title = element.Name,
				Subtitle = element.Tag,
				Text = element.Description,
				Image = new ThumbnailUrl
				{
					Url = DEFAULT_VIDEO_IMAGE
				},
				Media = new List<MediaUrl>
				{
					new MediaUrl()
					{
						Url = element.GetContentUrl()
					}
				}
			};

			return videoCard.ToAttachment();
		}

		private static Attachment GetAudioCard(MediaElement element)
		{
			var audioCard = new AudioCard
			{
				Title = element.Name,
				Subtitle = element.Tag,
				Text = element.Description,
				Media = new List<MediaUrl>
				{
					new MediaUrl()
					{
						Url = element.GetContentUrl()
					}
				}
			};

			return audioCard.ToAttachment();
		}

		private static Attachment GetGeneralCard(MediaElement element)
		{
			var heroCard = new HeroCard
			{
				Title = element.Name,
				Subtitle = element.Tag,
				Text = element.Description,
				Images = new List<CardImage>() { new CardImage(url: DEFAULT_DOC_IMAGE) }
			};

			return heroCard.ToAttachment();
		}

		private IEnumerable<MediaElement> SearchAttachmentsByTag(IMessageActivity message)
		{
			var tags = message.Text.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);

			var result = _user.MediaElements.Where(x => tags.Any(t => x.Tag.ToLower().Contains(t.ToLower())));

			return result;
		}

		private MediaElement SearchAttachmentsByName(IMessageActivity message)
		{
			var result = _user.MediaElements.FirstOrDefault(x => message.Text.ToLower() == x.Name.ToLower());

			return result;
		}
		
		private Attachment Map(MediaElement element)
		{
			return new Attachment
			{
				ContentType = element.ContentType,
				ContentUrl = element.GetContentUrl(),
				Name = element.Name
			};
		}
	}
}