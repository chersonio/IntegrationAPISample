using System.Text.Json.Serialization;

namespace IntegrationAPISample.Domain;

public class HotelResponse
{
    [JsonPropertyName("results")]
    public Results Results { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class Results
{
    [JsonPropertyName("locations")]
    public List<Location> Locations { get; set; }

    [JsonPropertyName("hotels")]
    public List<Hotel> Hotels { get; set; }
}

public class Hotel
{
    [JsonPropertyName("locationName")]
    public string LocationName { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public Coordinates Coordinates { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;
}

public class Location
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("cityName")]
    public string CityName { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public Coordinates Coordinates { get; set; }

    [JsonPropertyName("countryName")]
    public string CountryName { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; } = string.Empty;

    [JsonPropertyName("hotelsCount")]
    public string HotelsCount { get; set; } = string.Empty;
}

public class Coordinates
{
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}
