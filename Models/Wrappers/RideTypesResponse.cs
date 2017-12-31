using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    internal struct RideTypesResponse
    {
        [JsonProperty]
        public List<RideType> RideTypes { get; private set; }
    }
}
