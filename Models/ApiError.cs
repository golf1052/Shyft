using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shyft.Models
{
    public class ApiError
    {
        [JsonProperty]
        public string Error { get; private set; }

        [JsonProperty]
        public List<JObject> ErrorDetail { get; private set; }

        [JsonProperty]
        public string ErrorDescription { get; private set; }
    }
}
