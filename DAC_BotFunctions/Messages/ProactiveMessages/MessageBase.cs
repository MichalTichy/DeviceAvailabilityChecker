using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_BotFunctions.Subscription;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public abstract class MessageBase : IMessage
    {
        public async void Send()
        {
            var message = await Build();
            if (message == null)
                return;

            var recipients = await GetRecipients();
            foreach (var recipient in recipients)
            {
                await SendMessageToChannel("Unavailable devices detected!", message, recipient);
            }
        }
        
        protected async Task SendMessageToChannel(string title, string message, BotSubscription subscription)
        {
            var channelData = new TeamsChannelData
            {
                Channel = new ChannelInfo(subscription.ChannelId),
                Team = new TeamInfo(subscription.TeamId),
                Tenant = new TenantInfo(subscription.TenantId)
            };

            var newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Text = message;
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

        public abstract Task<string> Build();
        public abstract Task<IEnumerable<BotSubscription>> GetRecipients();
    }
}