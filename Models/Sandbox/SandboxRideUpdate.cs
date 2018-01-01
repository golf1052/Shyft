using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models.Sandbox
{
    public class SandboxRideUpdate
    {
        [JsonProperty]
        public string RideId { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideStatusConverter))]
        public LyftConstants.RideStatus Status { get; private set; }
    }
}
