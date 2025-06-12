namespace Unicornus.Core.Models;

public class Investor
{
    public int InvestorId { get; set; }
    public string InvestorName { get; set; } = string.Empty;
    
    public ICollection<CompanyInvestor> CompanyInvestors { get; set; } = new List<CompanyInvestor>();
}