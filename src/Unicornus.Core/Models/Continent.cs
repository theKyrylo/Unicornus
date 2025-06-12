using System.ComponentModel.DataAnnotations;

namespace Unicornus.Core.Models
{
    public class Continent
    {
        public int ContinentId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ContinentName { get; set; } = string.Empty;
        
        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}