using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Shyft.Models;

namespace Shyft.Helpers
{
    public class RideProfileConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = ((string)reader.Value).ToLower();
            var profiles = Enum.GetValues(typeof(LyftConstants.RideProfile)).Cast<LyftConstants.RideProfile>();
            foreach (var status in profiles)
            {
                if (status.ToString().ToLower() == value)
                {
                    return status;
                }
            }
            return LyftConstants.RideProfile.Other;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
