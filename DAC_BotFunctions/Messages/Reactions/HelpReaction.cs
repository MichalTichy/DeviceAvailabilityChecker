using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAC_BotFunctions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class HelpReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {
            var sb=new StringBuilder();
            var commands = GetCommandTypes().Select(CreateInstanceOfCommand);
            foreach (var command in commands)
            {
                sb.AppendLine(command.GetHelpText());
            }

            await context.PostAsync(sb.ToString().FixNewLines());
        }

        public string GetHelpText()
        {
            return "/help - displays this help text";
        }

        protected IEnumerable<Type> GetCommandTypes()
        {
            return typeof(HelpReaction).Assembly.GetTypes()
                .Where(t => t.FindInterfaces((type, criteria) => type==typeof(IReaction),null).Any())
                .OrderBy(t=>t.Name);
        }

        protected IReaction CreateInstanceOfCommand(Type commandType)
        {
            return (IReaction) Activator.CreateInstance(commandType);
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 1 && messageParts.First() == "/help";
        }
    }
}