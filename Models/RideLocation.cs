using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class RideLocation : Location
    {
        [JsonProperty]
        public int? EtaSeconds { get; private set; }
    }
}
