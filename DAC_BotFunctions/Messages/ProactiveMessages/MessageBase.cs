using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_Common;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public abstract class MessageBase : IMessage
    {
        public async Task SendMessageToChannel(string title, BotSubscription subscription)
        {
            var channelData = new TeamsChannelData
            {
                Channel = new ChannelInfo(subscription.ChannelId),
                Team = new TeamInfo(subscription.TeamId),
                Tenant = new TenantInfo(subscription.TenantId)
            };

            var newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            var newMessageText = await Build();
            newMessage.Text = newMessageText.FixNewLines();
            var conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: title,
                activity: (Activity)newMessage,
                channelData: channelData);

            var connector = new ConnectorClient(new Uri(subscription.ServiceUrl), Environment.GetEnvironmentVariable("MicrosoftAppId"), Environment.GetEnvironmentVariable("MicrosoftAppPassword"));
            MicrosoftAppCredentials.TrustServiceUrl(subscription.ServiceUrl, DateTime.MaxValue);
            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
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