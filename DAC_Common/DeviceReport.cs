using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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