using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class RideReceipt
    {
        [JsonProperty]
        public string RideId { get; private set; }

        [JsonProperty]
        public Cost Price { get; private set; }

        [JsonProperty]
        public List<LineItem> LineItems { get; private set; }

        [JsonProperty]
        public List<Charge> Charges { get; private set; }

        [JsonProperty]
        public DateTimeOffset RequestedAt { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideProfileConverter))]
        public LyftConstants.RideProfile RideProfile { get; private set; }
    }
}
