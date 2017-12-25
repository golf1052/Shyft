using Newtonsoft.Json;

namespace Shyft.Models
{
    public class PricingDetails
    {
        [JsonProperty]
        public int BaseCharge { get; private set; }

        [JsonProperty]
        public int CancelPenaltyAmount { get; private set; }

        [JsonProperty]
        public int CostMinimum { get; private set; }

        [JsonProperty]
        public int CostPerMile { get; private set; }

        [JsonProperty]
        public int CostPerMinute { get; private set; }

        [JsonProperty]
        public string Currency { get; private set; }

        [JsonProperty]
        public int TrustAndService { get; private set; }
    }
}
