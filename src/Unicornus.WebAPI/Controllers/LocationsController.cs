using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicornus.Infrastructure.Data;

namespace Unicornus.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly UnicornDbContext _context;

        public LocationsController(UnicornDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all locations with country and continent information
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.Locations
                .Include(l => l.Country)
                .Include(l => l.Continent)
                .Select(l => new
                {
                    l.LocationId,
                    l.City,
                    CountryName = l.Country.CountryName,
                    ContinentName = l.Continent.ContinentName
                })
                .ToListAsync();

            return Ok(locations);
        }
    }
}