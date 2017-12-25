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
                return RideTypeEnum.RideTypes.Lyft;
            }
            else if (value == "lyft_line")
            {
                return RideTypeEnum.RideTypes.LyftLine;
            }
            else if (value == "lyft_plus")
            {
                return RideTypeEnum.RideTypes.LyftPlus;
            }
            else if (value == "lyft_premier")
            {
                return RideTypeEnum.RideTypes.LyftPremier;
            }
            else if (value == "lyft_lux")
            {
                return RideTypeEnum.RideTypes.LyftLux;
            }
            else if (value == "lyft_luxsuv")
            {
                return RideTypeEnum.RideTypes.LyftLuxSuv;
            }
            else
            {
                return RideTypeEnum.RideTypes.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            RideTypeEnum.RideTypes rideType = (RideTypeEnum.RideTypes)value;
            writer.WriteValue(ShyftConstants.RideTypeToString(rideType));
        }
    }
}
