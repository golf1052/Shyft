using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shyft.Models
{
    public class DriverDetail
    {
        [JsonProperty]
        public string FirstName { get; private set; }

        [JsonProperty]
        public string PhoneNumber { get; private set; }

        [JsonProperty]
        public string Rating { get; private set; }

        [JsonProperty]
        public string ImageUrl { get; private set; }

        [JsonProperty]
        public string UserId { get; private set; }
    }
}
