using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_DAL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class UnregisterDeviceReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {
            var group = message.GetMessageParts().ElementAt(1);
            var address = message.GetMessageParts().ElementAt(2);
            try
            {
                await UnregisterDevice(message, group,address);
                await context.PostAsync("The Device has been unregistered successfully.");
            }
            catch (Exception  )
            {
                await context.PostAsync($"Device unregistraion failed!");
                throw;
            }
        }

        public string GetHelpText()
        {
            return "/unregisterDevice {group} {address} - deletes given device";
        }

        private async Task UnregisterDevice(IMessageActivity message, string group, string address)
        {
            var dataSource = new DataSource();
            await dataSource.Init();

            await dataSource.UnregisterDevice(group, address);
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 3 && messageParts.First() == "/unregisterDevice";
        }
    }
}