using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Shyft.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shyft.Models.Wrappers;

namespace Shyft
{
    public class ShyftClient
    {
        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        private string AccessToken { get; set; }

        public string RefreshToken { get; private set; }

        private DateTime ExpiresAt { get; set; }
        
        private AuthTypes AuthType { get; set; }

        private HttpClient httpClient;

        private JsonSerializerSettings jsonSettings;

        public enum AuthTypes
        {
            TwoLeg,
            ThreeLeg
        }

        public ShyftClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            httpClient = new HttpClient();
            jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(true, false)
                }
            };
        }

        public string GetAuthUrl(List<ShyftConstants.AuthScopes> scopes, string state = null)
        {
            string scopesString = null;
            if (scopes != null && scopes.Count > 0)
            {
                List<string> scopesList = new List<string>();
                foreach (var scope in scopes)
                {
                    scopesList.Add(ShyftConstants.AuthScopeToString(scope));
                }
                scopesString = string.Join(" ", scopesList);
            }

            Url url = new Url(ShyftConstants.BaseUrl).AppendPathSegments("oauth", "authorize")
                .SetQueryParams(new
                {
                    client_id = ClientId,
                    response_type = "code",
                    scope = scopesString,
                    state = state
                }, Flurl.NullValueHandling.NameOnly);

            return url;
        }

        public async Task Auth(string code = null)
        {
            JObject authData;
            if (string.IsNullOrEmpty(code))
            {
                authData = JObject.FromObject(new
                {
                    grant_type = "client_credentials",
                    scopes = "public"
                });
                AuthType = AuthTypes.TwoLeg;
            }
            else
            {
                authData = JObject.FromObject(new
                {
                    grant_type = "authorization_code",
                    code = code
                });
                AuthType = AuthTypes.ThreeLeg;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());
            JObject responseObject = await PostLyftAuth(new Url(ShyftConstants.BaseUrl).AppendPathSegments("oauth", "token"), authData);
            ExpiresAt = DateTime.Now + TimeSpan.FromSeconds((int)responseObject["expires_in"]);
            AccessToken = (string)responseObject["access_token"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            if (AuthType == AuthTypes.ThreeLeg)
            {
                RefreshToken = (string)responseObject["refresh_token"];
            }
        }

        public async Task AuthWithRefreshToken(string refreshToken)
        {
            AuthType = AuthTypes.ThreeLeg;
            RefreshToken = refreshToken;
            await RefreshAccessToken();
        }

        public async Task<List<RideType>> RetrieveRideTypes(double lat, double lng)
        {
            return await RetrieveRideTypes(lat, lng, RideTypeEnum.RideTypes.Unknown);
        }

        public async Task<List<RideType>> RetrieveRideTypes(double lat, double lng, RideTypeEnum.RideTypes rideType)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("ridetypes")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                })
                .SetRideType(rideType);
            return (await GetLyft<RideTypesObject>(url)).RideTypes;
        }

        public async Task<List<Eta>> RetrieveDriverEta(double lat, double lng, double? destinationLat = null, double? destinationLng = null)
        {
            return await RetrieveDriverEta(lat, lng, destinationLat, destinationLng, RideTypeEnum.RideTypes.Unknown);
        }

        public async Task<List<Eta>> RetrieveDriverEta(double lat, double lng, double? destinationLat, double? destinationLng, RideTypeEnum.RideTypes rideType)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("eta")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                })
                .SetRideType(rideType);
            return (await GetLyft<EtaEstimatesObject>(url)).EtaEstimates;
        }

        public async Task<List<CostEstimate>> RetrieveRideEstimates(double startLat, double startLng, double? endLat = null, double? endLng = null)
        {
            return await RetrieveRideEstimates(startLat, startLng, endLat, endLng, RideTypeEnum.RideTypes.Unknown);
        }

        public async Task<List<CostEstimate>> RetrieveRideEstimates(double startLat, double startLng, double? endLat, double? endLng, RideTypeEnum.RideTypes rideType)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("cost")
                .SetQueryParams(new
                {
                    start_lat = startLat,
                    start_lng = startLng,
                    end_lat = endLat,
                    end_lng = endLng
                })
                .SetRideType(rideType);
            return (await GetLyft<CostEstimatesObject>(url)).CostEstimates;
        }

        public async Task<List<NearbyDriversByRideType>> RetrieveNearbyDrivers(double lat, double lng)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("drivers")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                });
            NearbyDriversObject nearbyDriversObject = await GetLyft<NearbyDriversObject>(url);
            return nearbyDriversObject.NearbyDrivers;
        }

        private async Task<T> GetLyft<T>(string url)
        {
            await CheckAccess();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync(), jsonSettings);
        }

        private async Task<JObject> PostLyftAuth(string url, JObject data)
        {
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new StringContent(data.ToString(), Encoding.UTF8, "application/json"));
            return await responseMessage.ContentToJObject();
        }

        private async Task CheckAccess()
        {
            if (DateTime.Now >= ExpiresAt)
            {
                await RefreshAccessToken();
            }
        }

        private async Task RefreshAccessToken()
        {
            if (AuthType == AuthTypes.ThreeLeg)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());
                JObject authData = JObject.FromObject(new
                {
                    grant_type = "refresh_token",
                    refresh_token = RefreshToken
                });
                JObject responseObject = await PostLyftAuth(new Url(ShyftConstants.BaseUrl).AppendPathSegments("oauth", "token"), authData);
                ExpiresAt = DateTime.Now + TimeSpan.FromSeconds((int)responseObject["expires_in"]);
                AccessToken = (string)responseObject["access_token"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
        }

        private string GetBasicAuth()
        {
            byte[] authBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            return Convert.ToBase64String(authBytes);
        }
    }
}
