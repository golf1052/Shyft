using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class LatLng
    {
        [JsonProperty]
        public double Lat { get; private set; }

        [JsonProperty]
        public double Lng { get; private set; }
    }
}
