using System;

namespace DAC_DAL
{
    public class EnvironmentVariables
    {

        public static string StorageAccount = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    }
}
