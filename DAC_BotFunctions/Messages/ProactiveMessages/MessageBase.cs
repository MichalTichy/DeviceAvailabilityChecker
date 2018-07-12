using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_Common;
using DAC_DAL;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public abstract class MessageBase : IMessage
    {
        public async Task SendMessageToChannel(string title, BotSubscription subscription)
        {
            var subscriptionFacade = new SubscriptionFacade();
            var channelData = new TeamsChannelData
            {
                Channel = new ChannelInfo(subscription.ChannelId),
                Team = new TeamInfo(subscription.TeamId),
                Tenant = new TenantInfo(subscription.TenantId)
            };

            var newMessageText = await Build();

            var newMessage = new Activity
            {
                Type = ActivityTypes.Message,
                Text = newMessageText.FixNewLines(),
            };
            var conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: title,
                activity: (Activity)newMessage,
                channelData: channelData);

            var connector = new ConnectorClient(new Uri(subscription.ServiceUrl), Environment.GetEnvironmentVariable("MicrosoftAppId"), Environment.GetEnvironmentVariable("MicrosoftAppPassword"));
            MicrosoftAppCredentials.TrustServiceUrl(subscription.ServiceUrl, DateTime.MaxValue);
            
            if (subscription.LastActivity == null)
            {
                var result = await connector.Conversations.CreateConversationAsync(conversationParams);
                subscription.LastActivity = new LastActivity
                {
                    ConversationId = result.Id,
                    ActitityId = result.ActivityId
                };
            }
            else
            {
                var result = await connector.Conversations.UpdateActivityAsync(subscription.LastActivity.ConversationId,subscription.LastActivity.ActitityId,newMessage);
                subscription.LastActivity.ActitityId = result.Id;
            }

            await subscriptionFacade.UpdateBotSubscription(subscription);

        }

        public async Task SendMessageToChannel(string title, IEnumerable<BotSubscription> subscriptions)
        {
            foreach (var botSubscription in subscriptions)
            {
                await SendMessageToChannel(title, botSubscription);
            }
        }

        public abstract Task<string> Build();
    }
}