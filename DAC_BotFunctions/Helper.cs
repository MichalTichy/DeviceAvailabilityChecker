using System;
using DAC_BotFunctions.Subscription;

namespace DAC_BotFunctions
{
    public static class Helper
    {
        public static BotConfigurationStore ConfigStore { get; }

        static Helper()
        {
            ConfigStore = new BotConfigurationStore(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "config");
        }
    }
}
