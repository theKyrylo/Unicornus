using System.ComponentModel.DataAnnotations;

namespace Unicornus.Core.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        public int? YearFounded { get; set; }
        
        public decimal? ValuationInBillions { get; set; }
        
        public DateTime? DateJoinedUnicorn { get; set; }
        
        public decimal? FundingAmount { get; set; }
        
        [StringLength(1)]
        public string? FundingUnit { get; set; }
        
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
        
        public int IndustryId { get; set; }
        public Industry Industry { get; set; } = null!;
        
        public ICollection<CompanyInvestor> CompanyInvestors { get; set; } = new List<CompanyInvestor>();
    }
}