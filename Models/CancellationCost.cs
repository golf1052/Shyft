using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class CancellationCost : Cost
    {
        [JsonProperty]
        public string Token { get; private set; }

        [JsonProperty]
        public int TokenDuration { get; private set; }
    }
}
