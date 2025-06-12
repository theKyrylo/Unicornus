namespace Unicornus.Core.Models;

public class Industry
{
    public int IndustryId { get; set; }
    public string IndustryName { get; set; } = string.Empty;
    
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}