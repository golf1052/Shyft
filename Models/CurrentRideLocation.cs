using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class CurrentRideLocation : LatLng
    {
        [JsonProperty]
        public double? Bearing { get; private set; }
    }
}
