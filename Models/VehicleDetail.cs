using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class VehicleDetail
    {
        [JsonProperty]
        public string Make { get; private set; }

        [JsonProperty]
        public string Model { get; private set; }

        [JsonProperty]
        public string Year { get; private set; }

        [JsonProperty]
        public string LicensePlate { get; private set; }

        [JsonProperty]
        public string LicensePlateState { get; private set; }

        [JsonProperty]
        public string Color { get; private set; }

        [JsonProperty]
        public string ImageUrl { get; private set; }
    }
}
