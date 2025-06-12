using Microsoft.EntityFrameworkCore;
using Unicornus.Core.Interfaces;
using Unicornus.Core.Models;
using Unicornus.Infrastructure.Data;

namespace Unicornus.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly UnicornusDbContext _context;

    public CompanyRepository(UnicornusDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
    {
        return await _context.Companies
            .Include(c => c.Location)
            .ThenInclude(l => l.Country)
            .Include(c => c.Location)
            .ThenInclude(l => l.Continent)
            .Include(c => c.Industry)
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.Location)
            .ThenInclude(l => l.Country)
            .Include(c => c.Location)
            .ThenInclude(l => l.Continent)
            .Include(c => c.Industry)
            .Include(c => c.CompanyInvestors)
            .ThenInclude(ci => ci.Investor)
            .FirstOrDefaultAsync(c => c.CompanyId == id);
    }
}