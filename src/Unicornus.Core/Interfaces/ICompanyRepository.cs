using Unicornus.Core.Models;

namespace Unicornus.Core.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(int id);
}