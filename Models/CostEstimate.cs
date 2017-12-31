using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class CostEstimate
    {
        [JsonProperty]
        [JsonConverter(typeof(RideTypeConverter))]
        public LyftConstants.RideType RideType { get; private set; }

        [JsonProperty]
        public string DisplayName { get; private set; }

        [JsonProperty]
        public string Currency { get; private set; }

        [JsonProperty]
        public int EstimatedCostCentsMin { get; private set; }

        [JsonProperty]
        public int EstimatedCostCentsMax { get; private set; }

        [JsonProperty]
        public double EstimatedDistanceMiles { get; private set; }

        [JsonProperty]
        public int EstimatedDurationSeconds { get; private set; }

        [JsonProperty]
        public bool IsValidEstimate { get; private set; }

        [JsonProperty]
        public string PrimetimePercentage { get; private set; }

        [JsonProperty]
        public string CostToken { get; private set; }
    }
}
