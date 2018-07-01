using System;

namespace DAC_Common
{
    public static class EnvironmentVariables
    {
        public static string ApiUrl => Environment.GetEnvironmentVariable("apiUrl");

        public static double DelayInMinutesBetweenContacts => Convert.ToDouble(Environment.GetEnvironmentVariable("minutesBetweenContacts"));
        public static int NumberOfAllowedMissedContacts => Convert.ToInt32(Environment.GetEnvironmentVariable("numberOfAllowedMissedContacts"));
    }
}
