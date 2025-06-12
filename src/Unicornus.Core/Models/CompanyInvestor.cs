namespace Unicornus.Core.Models
{
    public class CompanyInvestor
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        
        public int InvestorId { get; set; }
        public Investor Investor { get; set; } = null!;
    }
}