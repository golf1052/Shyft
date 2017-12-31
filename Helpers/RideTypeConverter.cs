using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Models;

namespace Shyft.Helpers
{
    public class RideTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            if (value == "lyft")
            {
                return LyftConstants.RideType.Lyft;
            }
            else if (value == "lyft_line")
            {
                return LyftConstants.RideType.LyftLine;
            }
            else if (value == "lyft_plus")
            {
                return LyftConstants.RideType.LyftPlus;
            }
            else if (value == "lyft_premier")
            {
                return LyftConstants.RideType.LyftPremier;
            }
            else if (value == "lyft_lux")
            {
                return LyftConstants.RideType.LyftLux;
            }
            else if (value == "lyft_luxsuv")
            {
                return LyftConstants.RideType.LyftLuxSuv;
            }
            else
            {
                return LyftConstants.RideType.Other;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            LyftConstants.RideType rideType = (LyftConstants.RideType)value;
            writer.WriteValue(ShyftConstants.RideTypeToString(rideType));
        }
    }
}
