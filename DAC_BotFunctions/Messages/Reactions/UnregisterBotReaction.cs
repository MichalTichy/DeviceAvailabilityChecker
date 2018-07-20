using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_DAL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class UnregisterBotReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {
            var subscriptionId = message.GetMessageParts().ElementAt(1);
            await UnregisterSubscription(message, subscriptionId);
        }

        public string GetHelpText()
        {
            return "/unregisterBot {group} - deletes given subscription";
        }

        private async Task UnregisterSubscription(IMessageActivity message, string group)
        {
            var subscriptionFacade = new SubscriptionFacade();
            string teamId = message.ChannelData.team.id;
            await subscriptionFacade.UnregisterBotSubscription(teamId, group);
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 2 && messageParts.First() == "/unregisterBot";
        }
    }
}