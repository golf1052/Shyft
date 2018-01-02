using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shyft.Helpers;

namespace Shyft.Models
{
    public class RideDetail
    {
        [JsonProperty]
        public string RideId { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideStatusConverter))]
        public LyftConstants.RideStatus Status { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideTypeConverter))]
        public LyftConstants.RideType RideType { get; private set; }

        [JsonProperty]
        public PassengerDetail Passenger { get; private set; }

        [JsonProperty]
        public DriverDetail Driver { get; private set; }

        [JsonProperty]
        public VehicleDetail Vehicle { get; private set; }

        [JsonProperty]
        public RideLocation Origin { get; private set; }

        [JsonProperty]
        public RideLocation Destination { get; private set; }

        [JsonProperty]
        public PickupDropoffLocation Pickup { get; private set; }

        [JsonProperty]
        public PickupDropoffLocation Dropoff { get; private set; }

        [JsonProperty]
        public CurrentRideLocation Location { get; private set; }

        [JsonProperty]
        public string PrimetimePercentage { get; private set; }

        [JsonProperty]
        public double? DistanceMiles { get; private set; }

        [JsonProperty]
        public int? DurationSeconds { get; private set; }

        [JsonProperty]
        public Cost Price { get; private set; }

        [JsonProperty]
        public List<LineItem> LineItems { get; private set; }

        [JsonProperty]
        public List<string> CanCancel { get; private set; }

        [JsonProperty]
        public string CanceledBy { get; private set; }

        [JsonProperty]
        public CancellationCost CancellationPrice { get; private set; }

        [JsonProperty]
        public int? Rating { get; private set; }

        [JsonProperty]
        public string Feedback { get; private set; }

        [JsonProperty]
        public string PricingDetailsUrl { get; private set; }

        [JsonProperty]
        public string RouteUrl { get; private set; }

        [JsonProperty]
        public DateTimeOffset RequestedAt { get; private set; }

        [JsonProperty]
        public DateTimeOffset GeneratedAt { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(RideProfileConverter))]
        public LyftConstants.RideProfile RideProfile { get; private set; }

        [JsonProperty]
        public string BeaconColor { get; private set; }

        [JsonProperty]
        public List<Charge> Charges { get; private set; }
    }
}
