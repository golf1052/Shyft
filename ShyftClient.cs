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

        public async Task<RideRequest> RequestRide(double originLat, double originLng, double destinationLat, double destinationLng, LyftConstants.RideType rideType,
            string originAddress = null, string destinationAddress = null, string costToken = null)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("rides");
            JObject data = new JObject();
            data["origin"] = CreateLocation(originLat, originLng, originAddress);
            data["destination"] = CreateLocation(destinationLat, destinationLng, destinationAddress);
            data["ride_type"] = ShyftConstants.RideTypeToString(rideType);
            if (!string.IsNullOrEmpty(costToken))
            {
                data["cost_token"] = costToken;
            }
            return await PostLyft<RideRequest>(url, data);
        }

        private JObject CreateLocation(double lat, double lng, string address = null)
        {
            JObject o = new JObject();
            o["lat"] = lat;
            o["lng"] = lng;
            if (!string.IsNullOrEmpty(address))
            {
                o["address"] = address;
            }
            return o;
        }

        public async Task<RideDetail> RetrieveRideDetails(string rideId)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegments("rides", rideId);
            return await PostLyft<RideDetail>(url, null);
        }

        public async Task<Location> ChangeDestination(string rideId, double lat, double lng, string address = null)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegments("rides", rideId, "destination");
            JObject data = CreateLocation(lat, lng, address);
            return await PutLyft<Location>(url, data);
        }

        public async Task RateRide(string rideId, int rating, int tipAmount, string tipCurrency)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegments("rides", rideId, "rating");
            JObject data = JObject.FromObject(new
            {
                rating = rating,
                tip = new
                {
                    amount = tipAmount,
                    currency = tipCurrency
                }
            });
            await PutLyft(url, data);
        }

        public async Task<RideReceipt> RetrieveReceipt(string rideId)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegments("rides", rideId, "receipt");
            return await GetLyft<RideReceipt>(url);
        }

        public async Task CancelRide(string rideId, string cancelConfirmationToken = null)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegments("rides", rideId, "cancel");
            JObject data = null;
            if (cancelConfirmationToken != null)
            {
                data = JObject.FromObject(new
                {
                    cancel_confirmation_token = cancelConfirmationToken
                });
            }
            try
            {
                await PostLyft(url, data);
            }
            catch (ShyftException ex)
            {
                if (ex.ResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var cancellationCostError = JsonConvert.DeserializeObject<CancellationCostError>(await ex.ResponseMessage.Content.ReadAsStringAsync(), jsonSettings);
                    throw new LyftException<CancellationCostError>(cancellationCostError);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<ApiError>(await ex.ResponseMessage.Content.ReadAsStringAsync(), jsonSettings);
                    throw new LyftException<ApiError>(error);
                }
            }
        }

        public async Task<List<RideType>> RetrieveRideTypes(double lat, double lng)
        {
            return await RetrieveRideTypes(lat, lng, LyftConstants.RideType.Other);
        }

        public async Task<List<RideType>> RetrieveRideTypes(double lat, double lng, LyftConstants.RideType rideType)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("ridetypes")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                })
                .SetRideType(rideType);
            return (await GetLyft<RideTypesResponse>(url)).RideTypes;
        }

        public async Task<List<Eta>> RetrieveDriverEta(double lat, double lng, double? destinationLat = null, double? destinationLng = null)
        {
            return await RetrieveDriverEta(lat, lng, destinationLat, destinationLng, LyftConstants.RideType.Other);
        }

        public async Task<List<Eta>> RetrieveDriverEta(double lat, double lng, double? destinationLat, double? destinationLng, LyftConstants.RideType rideType)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("eta")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                })
                .SetRideType(rideType);
            return (await GetLyft<EtaEstimateResponse>(url)).EtaEstimates;
        }

        public async Task<List<CostEstimate>> RetrieveRideEstimates(double startLat, double startLng, double? endLat = null, double? endLng = null)
        {
            return await RetrieveRideEstimates(startLat, startLng, endLat, endLng, LyftConstants.RideType.Other);
        }

        public async Task<List<CostEstimate>> RetrieveRideEstimates(double startLat, double startLng, double? endLat, double? endLng, LyftConstants.RideType rideType)
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
            return (await GetLyft<CostEstimateResponse>(url)).CostEstimates;
        }

        public async Task<List<NearbyDriversByRideType>> RetrieveNearbyDrivers(double lat, double lng)
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("drivers")
                .SetQueryParams(new
                {
                    lat = lat,
                    lng = lng
                });
            NearbyDriversResponse nearbyDriversObject = await GetLyft<NearbyDriversResponse>(url);
            return nearbyDriversObject.NearbyDrivers;
        }

        public async Task<List<RideDetail>> RetrieveRideHistory(DateTimeOffset? startTime = null, DateTimeOffset? endTime = null, int limit = 10)
        {
            if (!startTime.HasValue)
            {
                startTime = new DateTimeOffset(new DateTime(2015, 1, 1), TimeSpan.Zero);
            }
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("rides")
                .SetQueryParams(new
                {
                    start_time = startTime.Value.ToUniversalTime().ToString(ShyftConstants.Iso8601Utc),
                    limit = limit
                });
            if (endTime.HasValue)
            {
                url.SetQueryParam("end_time", endTime.Value.ToUniversalTime().ToString(ShyftConstants.Iso8601Utc));
            }
            return (await GetLyft<RidesResponse>(url)).RideHistory;
        }

        public async Task<Profile> RetrieveProfile()
        {
            Url url = new Url(ShyftConstants.BaseV1Url).AppendPathSegment("profile");
            return await GetLyft<Profile>(url);
        }

        private async Task<T> GetLyft<T>(string url)
        {
            await CheckAccess();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync(), jsonSettings);
        }

        private async Task<T> PostLyft<T>(string url, JObject data)
        {
            await CheckAccess();
            string content = string.Empty;
            if (data != null)
            {
                content = data.ToString();
            }
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync(), jsonSettings);
        }

        private async Task PostLyft(string url, JObject data)
        {
            await CheckAccess();
            string content = string.Empty;
            if (data != null)
            {
                content = data.ToString();
            }
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new ShyftException(responseMessage);
            }
        }

        private async Task<T> PutLyft<T>(string url, JObject data)
        {
            await CheckAccess();
            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, new StringContent(data.ToString(), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync(), jsonSettings);
        }

        private async Task PutLyft(string url, JObject data)
        {
            await CheckAccess();
            HttpResponseMessage responseMessage = await httpClient.PutAsync(url, new StringContent(data.ToString(), Encoding.UTF8, "application/json"));
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
