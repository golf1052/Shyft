using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json.Linq;
using Shyft.Models;

namespace Shyft
{
    public static class ShyftConstants
    {
        public const string BaseUrl = "https://api.lyft.com/";

        public const string BaseV1Url = "https://api.lyft.com/v1/";

        public enum AuthScopes
        {
            Public,
            RidesRead,
            Offline,
            RidesRequest,
            Profile
        }

        public static string AuthScopeToString(AuthScopes scope)
        {
            if (scope == AuthScopes.Public)
            {
                return "public";
            }
            else if (scope == AuthScopes.RidesRead)
            {
                return "rides.read";
            }
            else if (scope == AuthScopes.Offline)
            {
                return "offline";
            }
            else if (scope == AuthScopes.RidesRequest)
            {
                return "rides.request";
            }
            else if (scope == AuthScopes.Profile)
            {
                return "profile";
            }
            else
            {
                return "none";
            }
        }

        public static string RideTypeToString(RideTypeEnum.RideTypes rideType)
        {
            if (rideType == RideTypeEnum.RideTypes.Lyft)
            {
                return "lyft";
            }
            else if (rideType == RideTypeEnum.RideTypes.LyftLine)
            {
                return "lyft_line";
            }
            else if (rideType == RideTypeEnum.RideTypes.LyftPlus)
            {
                return "lyft_plus";
            }
            else if (rideType == RideTypeEnum.RideTypes.LyftPremier)
            {
                return "lyft_premier";
            }
            else if (rideType == RideTypeEnum.RideTypes.LyftLux)
            {
                return "lyft_lux";
            }
            else if (rideType == RideTypeEnum.RideTypes.LyftLuxSuv)
            {
                return "lyft_luxsuv";
            }
            else
            {
                return "none";
            }
        }

        public static async Task<JObject> ContentToJObject(this HttpResponseMessage httpResponseMessage)
        {
            return JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        internal static Url SetRideType(this Url url, RideTypeEnum.RideTypes rideType)
        {
            if (rideType != RideTypeEnum.RideTypes.Unknown)
            {
                url.SetQueryParam("ride_type", ShyftConstants.RideTypeToString(rideType));
            }
            return url;
        }
    }
}
