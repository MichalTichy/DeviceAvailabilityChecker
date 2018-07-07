using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_BotFunctions.Subscription;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class RegisterBotReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {

            var groupName = message.GetMessageParts().ElementAt(1);
            await RegisterSubscription(message, groupName);
            await context.PostAsync("The subscription has been registered successfully.");
        }

        public string GetHelpText()
        {
            return "/registerBot {groupName} - registers bot to current channel";
        }

        protected async Task RegisterSubscription(IMessageActivity message, string groupName)
        {
            var config = await Helper.ConfigStore.Load();
            config.Subscriptions.Add(new BotSubscription()
            {
                ChannelId = message.ChannelData.channel.id,
                TeamId = message.ChannelData.team.id,
                TenantId = message.ChannelData.tenant.id,
                ServiceUrl = message.ServiceUrl,
                Id = Guid.NewGuid(),
                GroupName = groupName
            });
            await Helper.ConfigStore.Save(config);
        }

        public static bool IsValidReaction(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 2 && messageParts.First() == "/registerBot";
        }
    }
}