using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class UnregisterBotReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {
            var subscriptionId = message.GetMessageParts().ElementAt(1);
            var result = await UnregisterSubscription(message, subscriptionId);
            if (result)
            {
                await context.PostAsync("The subscription has been unregistered successfully.");
            }
            else
            {
                await context.PostAsync($"The subscription with Id {subscriptionId} was not found!");
            }
        }

        public string GetHelpText()
        {
            return "/unregisterBot {subscriptionId} - deletes given subscription";
        }

        private async Task<bool> UnregisterSubscription(IMessageActivity message, string subscriptionId)
        {
            var config = await Helper.ConfigStore.Load();
            var result = config.Subscriptions.RemoveAll(s => string.Equals(s.Id.ToString(), subscriptionId, StringComparison.CurrentCultureIgnoreCase));
            await Helper.ConfigStore.Save(config);
            return result > 0;
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 2 && messageParts.First() == "/unregisterBot";
        }
    }
}