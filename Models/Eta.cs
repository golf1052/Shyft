using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class Eta
    {
        [JsonProperty]
        [JsonConverter(typeof(RideTypeConverter))]
        public LyftConstants.RideType RideType { get; private set; }

        [JsonProperty]
        public string DisplayName { get; private set; }

        [JsonProperty]
        public int? EtaSeconds { get; private set; }

        [JsonProperty]
        public int? EtaSecondsMax { get; private set; }

        [JsonProperty]
        public bool IsValidEstimate { get; private set; }
    }
}
