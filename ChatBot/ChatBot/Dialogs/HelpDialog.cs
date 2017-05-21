using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ChatBot.Resources;
using ChatBot.Repository;
using ChatBot.Infrastructure;
using DataAccess.Models;
using System.Linq;
using System.Collections.Generic;

namespace ChatBot.Dialogs
{
	[Serializable]
	public class HelpDialog : IDialog<int>
	{
		private const string HELP_COMMAND = "Help";

		private IEnumerable<Help> _helpCommands;
		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
		}

		public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
		{
			UpdateHelpCommandsList();

			var message = await result;
			string helpMessage;

			var splitCommand = message.Text.ToLower().Split(new char[] {' '});

			if (splitCommand.Length == 1)
			{
				helpMessage = string.Format(ChatBotResources.HELP_DESCRIPTION, string.Join(", ", _helpCommands.Select(x => x.Command)));
			}

			else
			{
				var command = _helpCommands.FirstOrDefault(x => x.Command.ToLower() == splitCommand.Last());

				if (command == null)
				{
					helpMessage = ChatBotResources.HELP_DIDNT_FIND_COMMAND;
				}
				else
				{
					helpMessage = $"{command.Command} - {command.Description}";
				}
			}

			await context.PostAsync(helpMessage);
			context.Done(1);
		}

		private void UpdateHelpCommandsList()
		{
			if (_helpCommands == null)
			{
				using (var repository = new ChatBotRepository<Help>())
				{
					_helpCommands = repository.GetCollection;
				}
			}
		}
	}
}