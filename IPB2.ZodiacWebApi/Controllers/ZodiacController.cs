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
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.MyanmarMonth,
                    x.ZodiacSignImageUrl,
                    x.ZodiacSign2ImageUrl,
                    x.Dates,
                    x.Element,
                    x.ElementImageUrl,
                    x.LifePurpose,
                    x.Loyal,
                    x.RepresentativeFlower,
                    x.Angry,
                    x.Character,
                    x.PrettyFeatures,
                    Traits = x.Traits.Select(t => new
                    {
                        t.Name,
                        t.Percentage
                    }).ToList()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("horoscope")]
        public async Task<IActionResult> GetHoroscope(DateTime birthDate)
        {
            var day = birthDate.Day;
            var month = birthDate.Month;

            string zodiacName;

            if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
                zodiacName = "Aries";
            else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
                zodiacName = "Taurus";
            else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
                zodiacName = "Gemini";
            else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
                zodiacName = "Cancer";
            else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
                zodiacName = "Leo";
            else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
                zodiacName = "Virgo";
            else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
                zodiacName = "Libra";
            else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
                zodiacName = "Scorpio";
            else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
                zodiacName = "Sagittarius";
            else if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
                zodiacName = "Capricorn";
            else if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
                zodiacName = "Aquarius";
            else if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
                zodiacName = "Pisces";
            else
                return BadRequest("Invalid birth date.");

            // Fetch from database including traits
            //var zodiac = await _context.ZodiacSigns
            //                           .Include(x => x.Traits)
            //                           .FirstOrDefaultAsync(x => x.Name == zodiacName);

            //if (zodiac == null)
            //    return NotFound("Zodiac sign not found in database.");

            //return Ok(zodiac);
            var zodiac = await _context.ZodiacSigns
            .Where(x => x.Name == zodiacName)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.MyanmarMonth,
                x.ZodiacSignImageUrl,
                x.ZodiacSign2ImageUrl,
                x.Dates,
                x.Element,
                x.ElementImageUrl,
                x.LifePurpose,
                x.Loyal,
                x.RepresentativeFlower,
                x.Angry,
                x.Character,
                x.PrettyFeatures,
                Traits = x.Traits.Select(t => new
                {
                    t.Name,
                    t.Percentage
                })
            })
            .FirstOrDefaultAsync();

                    if (zodiac == null)
                        return NotFound("Zodiac sign not found");

                    return Ok(zodiac);
                }
    }
}
