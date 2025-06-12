using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicornus.Infrastructure.Data;

namespace Unicornus.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndustriesController : ControllerBase
    {
        private readonly UnicornDbContext _context;

        public IndustriesController(UnicornDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all industries
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetIndustries()
        {
            var industries = await _context.Industries
                .Select(i => new { i.IndustryId, i.IndustryName })
                .ToListAsync();

            return Ok(industries);
        }
    }
}