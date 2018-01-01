using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json.Linq;
using Shyft.Models;
using Shyft.Models.Sandbox;

namespace Shyft
{
    public class ShyftSandboxClient : ShyftClient
    {
        private static string SandboxPrefix = "SANDBOX-";

        public ShyftSandboxClient(string clientId, string clientSecret) : base(clientId, ConvertClientSecret(clientSecret))
        {
        }

        private static string ConvertClientSecret(string clientSecret)
        {
            if (!clientSecret.StartsWith(SandboxPrefix))
            {
                return $"{SandboxPrefix}{clientSecret}";
            }
            return clientSecret;
        }

        public async Task Auth(List<ShyftConstants.AuthScopes> scopes = null)
        {
            if (scopes == null)
            {
                scopes = new List<ShyftConstants.AuthScopes>()
                {
                    ShyftConstants.AuthScopes.Offline,
                    ShyftConstants.AuthScopes.Profile,
                    ShyftConstants.AuthScopes.Public,
                    ShyftConstants.AuthScopes.RidesRead,
                    ShyftConstants.AuthScopes.RidesRequest
                };
            }
            await Auth(null, scopes);
        }

        public async Task<SandboxRideUpdate> ChangeRideStatus(string rideId, LyftConstants.RideStatus status)
        {
            Url url = new Url(ShyftConstants.BaseV1SandboxUrl).AppendPathSegments("rides", rideId);
            JObject data = JObject.FromObject(new
            {
                status = ShyftConstants.RideStatusToString(status)
            });
            return (await base.PutLyft<SandboxRideUpdate>(url, data));
        }

        public async Task<SandboxRideType> ChangeRideTypes(double lat, double lng, List<LyftConstants.RideType> rideTypes)
        {
            Url url = new Url(ShyftConstants.BaseV1SandboxUrl).AppendPathSegment("ridetypes");
            JArray rideTypeA = new JArray();
            foreach (var rideType in rideTypes)
            {
                rideTypeA.Add(ShyftConstants.RideTypeToString(rideType));
            }
            JObject data = JObject.FromObject(new
            {
                lat = lat,
                lng = lng,
                ride_types = rideTypeA
            });
            return (await PutLyft<SandboxRideType>(url, data));
        }

        public async Task ChangePrimeTime(double lat, double lng, string primetimePercentage)
        {
            Url url = new Url(ShyftConstants.BaseV1SandboxUrl).AppendPathSegment("primetime");
            JObject data = JObject.FromObject(new
            {
                lat = lat,
                lng = lng,
                primetime_percentage = primetimePercentage
            });
            await PutLyft(url, data);
        }

        public async Task ChangeDriverAvailability(LyftConstants.RideType rideType, double lat, double lng, bool driverAvailability)
        {
            Url url = new Url(ShyftConstants.BaseV1SandboxUrl).AppendPathSegments("ridetypes", ShyftConstants.RideTypeToString(rideType));
            JObject data = JObject.FromObject(new
            {
                lat = lat,
                lng = lng,
                driver_availability = driverAvailability
            });
            await PutLyft(url, data);
        }
    }
}
