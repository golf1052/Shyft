using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class Location : LatLng
    {
        [JsonProperty]
        public string Address { get; private set; }
    }
}
