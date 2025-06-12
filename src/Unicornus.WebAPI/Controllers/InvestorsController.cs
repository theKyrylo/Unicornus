using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicornus.Infrastructure.Data;

namespace Unicornus.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestorsController : ControllerBase
    {
        private readonly UnicornDbContext _context;

        public InvestorsController(UnicornDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all investors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInvestors()
        {
            var investors = await _context.Investors
                .Select(i => new { i.InvestorId, i.InvestorName })
                .ToListAsync();

            return Ok(investors);
        }
    }
}