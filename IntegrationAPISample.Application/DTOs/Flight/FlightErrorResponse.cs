using System.Text.Json.Serialization;

namespace IntegrationAPISample.Application.DTOs.Flight;

public class FlightErrorResponse
{
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}

public class Error
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("context")]
    public Context Context { get; set; }
}

public class Context
{
    [JsonPropertyName("flight_date")]
    public List<FlightDate> FlightDate { get; set; }
}

public class FlightDate
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
