using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    public class RidesResponse
    {
        [JsonProperty]
        public List<RideDetail> RideHistory { get; private set; }
    }
}
