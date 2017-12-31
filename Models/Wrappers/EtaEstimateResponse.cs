using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    internal struct EtaEstimateResponse
    {
        [JsonProperty]
        public List<Eta> EtaEstimates { get; private set; }
    }
}
