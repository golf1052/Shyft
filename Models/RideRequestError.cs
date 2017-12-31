using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class RideRequestError : ApiError
    {
        [JsonProperty]
        public string PrimetimePercentage { get; private set; }

        [JsonProperty]
        public double PrimetimeMultiplier { get; private set; }

        [JsonProperty]
        public string PrimetimeConfirmationToken { get; private set; }

        [JsonProperty]
        public string CostToken { get; private set; }

        [JsonProperty]
        public int TokenDuration { get; private set; }

        [JsonProperty]
        public string ErrorUri { get; private set; }
    }
}
