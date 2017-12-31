using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class RideRequest
    {
        [JsonProperty]
        public string RideId { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideStatusConverter))]
        public LyftConstants.RideStatus Status { get; private set; }

        [JsonProperty]
        public Location Origin { get; private set; }

        [JsonProperty]
        public Location Destination { get; private set; }

        [JsonProperty]
        public PassengerDetail Passenger { get; private set; }
    }
}
