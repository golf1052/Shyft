using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    internal struct CostEstimateResponse
    {
        [JsonProperty]
        public List<CostEstimate> CostEstimates { get; private set; }
    }
}
