using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAC_DAL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class ListSubscriptionsBotReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {

            var result = await ListSubscriptions(message.GetChannelData<TeamsChannelData>().Team.Id);

            if (string.IsNullOrWhiteSpace(result))
            {
                await context.PostAsync("No subscriptions found.");
                return;
            }

            var text = "Listing all subscriptions: \n\n" + result;
            await context.PostAsync(text.FixNewLines());
        }

        public string GetHelpText()
        {
            return "/list - lists all subscriptions";
        }

        private async Task<string> ListSubscriptions(string messageChannelId)
        {
            var subscriptionFacade = new SubscriptionFacade();
            var sb = new StringBuilder();

            foreach (var subscription in await subscriptionFacade.GetAllSubscriptionsWithChannelId(messageChannelId))
            {
                sb.AppendLine($"{subscription.GroupName}");
            }

            return sb.ToString();
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 1 && messageParts.First() == "/list";
        }
    }
}