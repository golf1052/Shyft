using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class RideType
    {
        [JsonProperty("ride_type")]
        [JsonConverter(typeof(RideTypeConverter))]
        public LyftConstants.RideType Type { get; private set; }

        [JsonProperty]
        public string DisplayName { get; private set; }

        [JsonProperty]
        public int Seats { get; private set; }

        [JsonProperty]
        public string ImageUrl { get; private set; }

        [JsonProperty]
        public PricingDetails PricingDetails { get; private set; }

        [JsonProperty]
        public PricingDetails ScheduledPricingDetails { get; private set; }
    }
}
