namespace Unicornus.Core.Models;

public class Continent
{
    public int ContinentId { get; set; }
    public string ContinentName { get; set; } = string.Empty;
    
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}