using System;
using System.Threading.Tasks;
using DAC_BotFunctions.Messages.Reactions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;

namespace DAC_BotFunctions.Dialogs
{
    [Serializable]
    public class SystemCommandDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            try
            {
                context.Wait(MessageReceivedAsync);
            }
            catch (OperationCanceledException error)
            {
                return Task.FromCanceled(error.CancellationToken);
            }
            catch (Exception error)
            {
                return Task.FromException(error);
            }

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            IReaction reaction = null;
            if (RegisterBotReaction.IsValidReaction(message))
            {
                reaction = new RegisterBotReaction();
            }
            else if (UnregisterBotReaction.IsReactionValid(message))
            {
                reaction = new UnregisterBotReaction();
            }
            else if (ListSubscriptionsBotReaction.IsReactionValid(message))
            {
                reaction = new ListSubscriptionsBotReaction();
            }
            else if (RegisterDeviceBotReaction.IsReactionValid(message))
            {
                reaction = new RegisterDeviceBotReaction();
            }
            else if (ReportStatusReaction.IsValidReaction(message))
            {
                reaction=new ReportStatusReaction();
            }
            else
            {
                reaction = new HelpReaction();
            }

            reaction.Execute(context, message);

            context.Wait(MessageReceivedAsync);
        }
    }
}