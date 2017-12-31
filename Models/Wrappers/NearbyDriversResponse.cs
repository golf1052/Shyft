using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    internal struct NearbyDriversResponse
    {
        [JsonProperty]
        public List<NearbyDriversByRideType> NearbyDrivers { get; private set; }
    }
}
