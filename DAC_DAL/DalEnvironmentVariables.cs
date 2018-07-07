using System;

namespace DAC_DAL
{
    public class DalEnvironmentVariables
    {
        public static string StorageAccount = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    }
}
