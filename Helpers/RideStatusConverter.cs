using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Shyft.Models;

namespace Shyft.Helpers
{
    public class RideStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = ((string)reader.Value).ToLower();
            var statuses = Enum.GetValues(typeof(LyftConstants.RideStatus)).Cast<LyftConstants.RideStatus>();
            foreach (var status in statuses)
            {
                if (status.ToString().ToLower() == value)
                {
                    return status;
                }
            }
            return LyftConstants.RideStatus.Unknown;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            LyftConstants.RideStatus rideStatus = (LyftConstants.RideStatus)value;
            writer.WriteValue(ShyftConstants.RideStatusToString(rideStatus));
        }
    }
}
