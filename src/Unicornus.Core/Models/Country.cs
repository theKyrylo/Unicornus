namespace Unicornus.Core.Models;

public class Country
{
    public int CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}