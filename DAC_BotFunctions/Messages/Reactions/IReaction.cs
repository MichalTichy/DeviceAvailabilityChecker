using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions.Messages.Reactions
{
    public interface IReaction
    {
        void Execute(IDialogContext context, IMessageActivity message);
        string GetHelpText();
    }
}
