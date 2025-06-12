namespace Unicornus.Core.Models;

public class Location
{
    public int LocationId { get; set; }
    public string City { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int ContinentId { get; set; }
    
    public Country Country { get; set; } = null!;
    public Continent Continent { get; set; } = null!;
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}