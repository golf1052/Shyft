using System;
using System.Collections.Generic;
using System.Text;

namespace Shyft.Models
{
    public class LyftConstants
    {
        public enum RideType
        {
            Lyft,
            LyftLine,
            LyftPlus,
            LyftPremier,
            LyftLux,
            LyftLuxSuv,
            Other
        }

        public enum RideStatus
        {
            Pending,
            Accepted,
            Arrived,
            PickedUp,
            DroppedOff,
            Canceled,
            Scheduled,
            Unknown
        }

        public enum RideProfile
        {
            Personal,
            Business,
            Other
        }
    }
}
