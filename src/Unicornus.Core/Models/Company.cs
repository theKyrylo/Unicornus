namespace Unicornus.Core.Models;

public class Company
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int? YearFounded { get; set; }
    public decimal? ValuationInBillions { get; set; }
    public DateTime? DateJoinedUnicorn { get; set; }
    public decimal? FundingAmount { get; set; }
    public char? FundingUnit { get; set; }
    public int LocationId { get; set; }
    public int IndustryId { get; set; }
    
    public Location Location { get; set; } = null!;
    public Industry Industry { get; set; } = null!;
    public ICollection<CompanyInvestor> CompanyInvestors { get; set; } = new List<CompanyInvestor>();
}