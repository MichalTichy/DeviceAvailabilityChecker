using System.Linq;
using System.Threading.Tasks;
using DAC_BotFunctions;
using DAC_Common;
using DAC_DAL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public class RegisterDeviceBotReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {
            // register the bot in this channel
            var name = message.GetMessageParts().ElementAt(1);
            var address = message.GetMessageParts().ElementAt(2);
            var group = message.GetMessageParts().ElementAt(3);
            await RegisterDevice(context, message, name, address,group);
        }

        public string GetHelpText()
        {
            return "/registerDevice {name} {address} {group} - adds device to be watched";
        }

        private async Task RegisterDevice(IDialogContext context, IMessageActivity message, string name, string address, string group)
        {

            var device = new DeviceReport()
            {
                Address = address,
                Name = name,
                Group = group
            };
            var dataSource = new DataSource();
            await dataSource.Init();
            await dataSource.RegisterDevice(device);
            await context.PostAsync("Device registered.");
        }

        public static bool IsReactionValid(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 4 && messageParts.First() == "/registerDevice";
        }
    }
}