using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.DTOs;

public class WeatherDto
{
    public double Temperature { get; set; }
    
    public double FeelsLike { get; set; }
    
    public double TemperatureMin { get; set; }
    
    public double TemperatureMax { get; set; }
    
    public Wind Wind { get; set; }
    
    public List<Weather> Weather { get; set; }

    public string Error { get; set; } = string.Empty;
}
