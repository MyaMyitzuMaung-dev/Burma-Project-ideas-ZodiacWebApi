using IPB2.ZodiacWebApi.Database.AppDbContextModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.ZodiacWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ZodiacController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ZodiacSigns
                                     .Include(x => x.Traits)
                                     .ToListAsync();

            return Ok(data);
        }
    }
}
