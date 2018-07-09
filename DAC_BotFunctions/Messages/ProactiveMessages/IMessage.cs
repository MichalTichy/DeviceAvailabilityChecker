using System.Threading.Tasks;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public interface IMessage
    {
        Task<string> Build();
    }
}
