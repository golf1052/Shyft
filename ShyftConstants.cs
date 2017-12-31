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

        public const string Iso8601Utc = "yyyy-MM-ddTHH:mm:ssZ";

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

        public static string RideTypeToString(LyftConstants.RideType rideType)
        {
            if (rideType == LyftConstants.RideType.Lyft)
            {
                return "lyft";
            }
            else if (rideType == LyftConstants.RideType.LyftLine)
            {
                return "lyft_line";
            }
            else if (rideType == LyftConstants.RideType.LyftPlus)
            {
                return "lyft_plus";
            }
            else if (rideType == LyftConstants.RideType.LyftPremier)
            {
                return "lyft_premier";
            }
            else if (rideType == LyftConstants.RideType.LyftLux)
            {
                return "lyft_lux";
            }
            else if (rideType == LyftConstants.RideType.LyftLuxSuv)
            {
                return "lyft_luxsuv";
            }
            else
            {
                return "other";
            }
        }

        public static async Task<JObject> ContentToJObject(this HttpResponseMessage httpResponseMessage)
        {
            return JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        internal static Url SetRideType(this Url url, LyftConstants.RideType rideType)
        {
            if (rideType != LyftConstants.RideType.Other)
            {
                url.SetQueryParam("ride_type", ShyftConstants.RideTypeToString(rideType));
            }
            return url;
        }
    }
}
