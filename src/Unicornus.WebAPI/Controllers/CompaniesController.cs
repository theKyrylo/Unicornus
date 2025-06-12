using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicornus.Core.Models;
using Unicornus.Infrastructure.Data;
using Unicornus.WebAPI.DTOs;

namespace Unicornus.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly UnicornDbContext _context;

        public CompaniesController(UnicornDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _context.Companies
                .Include(c => c.Location)
                    .ThenInclude(l => l.Country)
                .Include(c => c.Location)
                    .ThenInclude(l => l.Continent)
                .Include(c => c.Industry)
                .Include(c => c.CompanyInvestors)
                    .ThenInclude(ci => ci.Investor)
                .Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    YearFounded = c.YearFounded,
                    ValuationInBillions = c.ValuationInBillions,
                    DateJoinedUnicorn = c.DateJoinedUnicorn,
                    FundingAmount = c.FundingAmount,
                    FundingUnit = c.FundingUnit,
                    LocationCity = c.Location.City,
                    CountryName = c.Location.Country.CountryName,
                    ContinentName = c.Location.Continent.ContinentName,
                    IndustryName = c.Industry.IndustryName,
                    InvestorNames = c.CompanyInvestors.Select(ci => ci.Investor.InvestorName).ToList()
                })
                .ToListAsync();

            return Ok(companies);
        }

        /// <summary>
        /// Get company by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Location)
                    .ThenInclude(l => l.Country)
                .Include(c => c.Location)
                    .ThenInclude(l => l.Continent)
                .Include(c => c.Industry)
                .Include(c => c.CompanyInvestors)
                    .ThenInclude(ci => ci.Investor)
                .Where(c => c.CompanyId == id)
                .Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    YearFounded = c.YearFounded,
                    ValuationInBillions = c.ValuationInBillions,
                    DateJoinedUnicorn = c.DateJoinedUnicorn,
                    FundingAmount = c.FundingAmount,
                    FundingUnit = c.FundingUnit,
                    LocationCity = c.Location.City,
                    CountryName = c.Location.Country.CountryName,
                    ContinentName = c.Location.Continent.ContinentName,
                    IndustryName = c.Industry.IndustryName,
                    InvestorNames = c.CompanyInvestors.Select(ci => ci.Investor.InvestorName).ToList()
                })
                .FirstOrDefaultAsync();

            if (company == null)
                return NotFound();

            return Ok(company);
        }

        /// <summary>
        /// Create a new company (requires authentication)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CreateCompanyDto createCompanyDto)
        {
            // Validate location exists
            var locationExists = await _context.Locations.AnyAsync(l => l.LocationId == createCompanyDto.LocationId);
            if (!locationExists)
                return BadRequest("Invalid location ID");

            // Validate industry exists
            var industryExists = await _context.Industries.AnyAsync(i => i.IndustryId == createCompanyDto.IndustryId);
            if (!industryExists)
                return BadRequest("Invalid industry ID");

            // Validate investors exist
            if (createCompanyDto.InvestorIds.Any())
            {
                var validInvestorIds = await _context.Investors
                    .Where(i => createCompanyDto.InvestorIds.Contains(i.InvestorId))
                    .Select(i => i.InvestorId)
                    .ToListAsync();

                if (validInvestorIds.Count != createCompanyDto.InvestorIds.Count)
                    return BadRequest("One or more investor IDs are invalid");
            }

            var company = new Company
            {
                CompanyName = createCompanyDto.CompanyName,
                YearFounded = createCompanyDto.YearFounded,
                ValuationInBillions = createCompanyDto.ValuationInBillions,
                DateJoinedUnicorn = createCompanyDto.DateJoinedUnicorn,
                FundingAmount = createCompanyDto.FundingAmount,
                FundingUnit = createCompanyDto.FundingUnit,
                LocationId = createCompanyDto.LocationId,
                IndustryId = createCompanyDto.IndustryId
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Add company-investor relationships
            if (createCompanyDto.InvestorIds.Any())
            {
                var companyInvestors = createCompanyDto.InvestorIds.Select(investorId => new CompanyInvestor
                {
                    CompanyId = company.CompanyId,
                    InvestorId = investorId
                }).ToList();

                _context.CompanyInvestors.AddRange(companyInvestors);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, await GetCompanyDto(company.CompanyId));
        }

        /// <summary>
        /// Update a company (requires authentication)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCompany(int id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyInvestors)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null)
                return NotFound();

            // Validate location exists
            var locationExists = await _context.Locations.AnyAsync(l => l.LocationId == updateCompanyDto.LocationId);
            if (!locationExists)
                return BadRequest("Invalid location ID");

            // Validate industry exists
            var industryExists = await _context.Industries.AnyAsync(i => i.IndustryId == updateCompanyDto.IndustryId);
            if (!industryExists)
                return BadRequest("Invalid industry ID");

            // Validate investors exist
            if (updateCompanyDto.InvestorIds.Any())
            {
                var validInvestorIds = await _context.Investors
                    .Where(i => updateCompanyDto.InvestorIds.Contains(i.InvestorId))
                    .Select(i => i.InvestorId)
                    .ToListAsync();

                if (validInvestorIds.Count != updateCompanyDto.InvestorIds.Count)
                    return BadRequest("One or more investor IDs are invalid");
            }

            // Update company properties
            company.CompanyName = updateCompanyDto.CompanyName;
            company.YearFounded = updateCompanyDto.YearFounded;
            company.ValuationInBillions = updateCompanyDto.ValuationInBillions;
            company.DateJoinedUnicorn = updateCompanyDto.DateJoinedUnicorn;
            company.FundingAmount = updateCompanyDto.FundingAmount;
            company.FundingUnit = updateCompanyDto.FundingUnit;
            company.LocationId = updateCompanyDto.LocationId;
            company.IndustryId = updateCompanyDto.IndustryId;

            // Update company-investor relationships
            _context.CompanyInvestors.RemoveRange(company.CompanyInvestors);
            
            if (updateCompanyDto.InvestorIds.Any())
            {
                var companyInvestors = updateCompanyDto.InvestorIds.Select(investorId => new CompanyInvestor
                {
                    CompanyId = company.CompanyId,
                    InvestorId = investorId
                }).ToList();

                _context.CompanyInvestors.AddRange(companyInvestors);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete a company (requires authentication)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound();

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<CompanyDto> GetCompanyDto(int companyId)
        {
            return await _context.Companies
                .Include(c => c.Location)
                    .ThenInclude(l => l.Country)
                .Include(c => c.Location)
                    .ThenInclude(l => l.Continent)
                .Include(c => c.Industry)
                .Include(c => c.CompanyInvestors)
                    .ThenInclude(ci => ci.Investor)
                .Where(c => c.CompanyId == companyId)
                .Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    YearFounded = c.YearFounded,
                    ValuationInBillions = c.ValuationInBillions,
                    DateJoinedUnicorn = c.DateJoinedUnicorn,
                    FundingAmount = c.FundingAmount,
                    FundingUnit = c.FundingUnit,
                    LocationCity = c.Location.City,
                    CountryName = c.Location.Country.CountryName,
                    ContinentName = c.Location.Continent.ContinentName,
                    IndustryName = c.Industry.IndustryName,
                    InvestorNames = c.CompanyInvestors.Select(ci => ci.Investor.InvestorName).ToList()
                })
                .FirstAsync();
        }
    }
}