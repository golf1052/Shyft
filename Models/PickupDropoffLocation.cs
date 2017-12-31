using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class PickupDropoffLocation : Location
    {
        [JsonProperty]
        public DateTimeOffset Time { get; private set; }
    }
}
