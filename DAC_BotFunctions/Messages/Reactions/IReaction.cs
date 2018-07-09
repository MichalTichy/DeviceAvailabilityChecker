using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_BotFunctions.Subscription;
using DAC_DAL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public interface IReaction
    {
        void Execute(IDialogContext context, IMessageActivity message);
        string GetHelpText();
    }

    public class ReportStatusReaction : IReaction
    {
        public async void Execute(IDialogContext context, IMessageActivity message)
        {

            var groupName = message.GetMessageParts().ElementAt(1);
            var messageContent = await ReportStatus(message, groupName);
            await context.PostAsync(messageContent.FixNewLines());
        }

        public string GetHelpText()
        {
            return "/reportStatus {groupName} - reports status of requested group";
        }

        protected async Task<string> ReportStatus(IMessageActivity message, string groupName)
        {
            var datasource = new DataSource();
            await datasource.Init();
            var devices = await datasource.GetDeviceReports();

            var replyMessage=new ProactiveMessages.DeviceStatusMessage(devices.Where(t=>t.Group==groupName));
            return await replyMessage.Build();

        }

        public static bool IsValidReaction(IMessageActivity message)
        {
            var messageParts = message.GetMessageParts().ToArray();
            return messageParts.Count() == 2 && messageParts.First() == "/reportStatus";
        }
    }
}
