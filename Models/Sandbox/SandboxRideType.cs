using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models.Sandbox
{
    public class SandboxRideType : LatLng
    {
        [JsonProperty]
        [JsonConverter(typeof(RideTypeConverter))]
        public List<LyftConstants.RideType> RideTypes { get; private set; }
    }
}
