using System;

namespace DAC_Common
{
    public class DeviceReport : Device
    {
        public DateTime? LastSeen { get; set; }

        public bool IsUnavailable => !LastSeen.HasValue ||
                                     LastSeen.Value.AddMinutes(
                                         EnvironmentVariables.DelayInMinutesBetweenContacts *
                                         EnvironmentVariables.NumberOfAllowedMissedContacts) < DateTime.Now;
    }
}