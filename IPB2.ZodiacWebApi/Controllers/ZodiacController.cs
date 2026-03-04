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

        // GET: api/zodiac
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await ProjectZodiac(_context.ZodiacSigns).ToListAsync();
            return Ok(data);
        }

        // GET: api/zodiac/horoscope?birthDate=1990-08-15
        [HttpGet("horoscope")]
        public async Task<IActionResult> GetHoroscope(DateTime birthDate)
        {
            var zodiacName = GetZodiacName(birthDate);
            if (zodiacName == null)
                return BadRequest("Invalid birth date.");

            var zodiac = await ProjectZodiac(_context.ZodiacSigns
                                      .Where(z => z.Name == zodiacName))
                                      .FirstOrDefaultAsync();

            if (zodiac == null)
                return NotFound("Zodiac sign not found");

            return Ok(zodiac);
        }

        // GET: api/zodiac/search?query=Cancer
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty.");

            var results = await ProjectZodiac(_context.ZodiacSigns
                                     .Where(z => EF.Functions.Like(z.Name, $"%{query}%")))
                                     .ToListAsync();

            if (!results.Any())
                return NotFound("No matching zodiac signs found.");

            return Ok(results);
        }

        private IQueryable<object> ProjectZodiac(IQueryable<ZodiacSign> query)
        {
            return query.Select(z => new
            {
                z.Id,
                z.Name,
                z.MyanmarMonth,
                z.ZodiacSignImageUrl,
                z.ZodiacSign2ImageUrl,
                z.Dates,
                z.Element,
                z.ElementImageUrl,
                z.LifePurpose,
                z.Loyal,
                z.RepresentativeFlower,
                z.Angry,
                z.Character,
                z.PrettyFeatures,
                Traits = z.Traits.Select(t => new
                {
                    t.Name,
                    t.Percentage
                })
            });
        }

        private string? GetZodiacName(DateTime birthDate)
        {
            int day = birthDate.Day;
            int month = birthDate.Month;

            return (month, day) switch
            {
                (3, >= 21) or (4, <= 19) => "Aries",
                (4, >= 20) or (5, <= 20) => "Taurus",
                (5, >= 21) or (6, <= 20) => "Gemini",
                (6, >= 21) or (7, <= 22) => "Cancer",
                (7, >= 23) or (8, <= 22) => "Leo",
                (8, >= 23) or (9, <= 22) => "Virgo",
                (9, >= 23) or (10, <= 22) => "Libra",
                (10, >= 23) or (11, <= 21) => "Scorpio",
                (11, >= 22) or (12, <= 21) => "Sagittarius",
                (12, >= 22) or (1, <= 19) => "Capricorn",
                (1, >= 20) or (2, <= 18) => "Aquarius",
                (2, >= 19) or (3, <= 20) => "Pisces",
                _ => null
            };
        }
    }
}