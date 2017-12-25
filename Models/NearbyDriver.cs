using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class NearbyDriver
    {
        [JsonProperty]
        public List<LatLng> Locations { get; private set; }
    }
}
