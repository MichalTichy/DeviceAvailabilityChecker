namespace DAC_Pinger
{
    public class Configuration
    {

        //DON`T FORGET TO CHANGE THIS SETTINGs ALSO IN OTHER PARTS OF PROJECT
        public const string ApiUrl = @"https://dacapifunctions.azurewebsites.net/";
        public const int Timeout = 1024;
        public const string Data = "Are you alive?";
        public const int TimeBetweenPings = 5;
    }
}