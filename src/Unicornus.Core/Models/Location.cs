using System.ComponentModel.DataAnnotations;

namespace Unicornus.Core.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;
        
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        
        public int ContinentId { get; set; }
        public Continent Continent { get; set; } = null!;
        
        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}