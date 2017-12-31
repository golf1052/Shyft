using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class CancellationCostError : ApiError
    {
        [JsonProperty]
        public int Amount { get; private set; }

        [JsonProperty]
        public string Currency { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public string Token { get; private set; }

        [JsonProperty]
        public int TokenDuration { get; private set; }
    }
}
