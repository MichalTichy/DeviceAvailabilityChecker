using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_BotFunctions.Subscription;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public interface IMessage
    {
        void Send();
        Task<IEnumerable<BotSubscription>> GetRecipients();
        Task<string> Build();
    }
}
