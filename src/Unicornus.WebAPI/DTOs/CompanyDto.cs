using System.ComponentModel.DataAnnotations;

namespace Unicornus.WebAPI.DTOs
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int? YearFounded { get; set; }
        public decimal? ValuationInBillions { get; set; }
        public DateTime? DateJoinedUnicorn { get; set; }
        public decimal? FundingAmount { get; set; }
        public string? FundingUnit { get; set; }
        public string LocationCity { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string ContinentName { get; set; } = string.Empty;
        public string IndustryName { get; set; } = string.Empty;
        public List<string> InvestorNames { get; set; } = new List<string>();
    }

    public class CreateCompanyDto
    {
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        public int? YearFounded { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? ValuationInBillions { get; set; }
        
        public DateTime? DateJoinedUnicorn { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? FundingAmount { get; set; }
        
        [RegularExpression("^[MB]$", ErrorMessage = "Funding unit must be 'M' or 'B'")]
        public string? FundingUnit { get; set; }
        
        [Required]
        public int LocationId { get; set; }
        
        [Required]
        public int IndustryId { get; set; }
        
        public List<int> InvestorIds { get; set; } = new List<int>();
    }

    public class UpdateCompanyDto
    {
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        public int? YearFounded { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? ValuationInBillions { get; set; }
        
        public DateTime? DateJoinedUnicorn { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? FundingAmount { get; set; }
        
        [RegularExpression("^[MB]$", ErrorMessage = "Funding unit must be 'M' or 'B'")]
        public string? FundingUnit { get; set; }
        
        [Required]
        public int LocationId { get; set; }
        
        [Required]
        public int IndustryId { get; set; }
        
        public List<int> InvestorIds { get; set; } = new List<int>();
    }
}