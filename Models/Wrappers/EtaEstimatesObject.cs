using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models.Wrappers
{
    internal struct EtaEstimatesObject
    {
        [JsonProperty]
        public List<Eta> EtaEstimates { get; private set; }
    }
}
