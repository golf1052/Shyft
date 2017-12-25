﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class NearbyDriversByRideType
    {
        [JsonConverter(typeof(RideTypeConverter))]
        public RideTypeEnum.RideTypes RideType { get; private set; }

        [JsonProperty]
        public List<NearbyDriver> Drivers { get; private set; }
    }
}