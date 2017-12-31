using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class Charge
    {
        [JsonProperty]
        public int Amount { get; private set; }

        [JsonProperty]
        public string Currency { get; private set; }

        [JsonProperty]
        public string PaymentMethod { get; private set; }
    }
}
