using System.ComponentModel.DataAnnotations;

namespace IntegrationAPISample.Application.DTOs
{
    public class AggregationQueryParams
    {
        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string CountryCode { get; set; } = string.Empty;

        [Required]
        public string SpecificDate { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Passengers { get; set; }
    }
}
