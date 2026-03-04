using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.ZodiacWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChineseZodiacController : ControllerBase
    {
        private static readonly string[] ZodiacAnimals = new[]
        {
            "Rat🐀", "Ox🐂", "Tiger🐅", "Rabbit🐇", "Dragon🐉", "Snake🐍",
            "Horse🐎", "Sheep🐑", "Monkey🐒", "Rooster🐓", "Dog🐕", "Pig🐖"
        };

        // GET: api/chinesezodiac?birthYear=1995
        [HttpGet]
        public IActionResult GetChineseZodiac([FromQuery] int birthYear)
        {
            if (birthYear <= 0)
                return BadRequest("Invalid birth year.");

            // Correct calculation using 1900 as base year (Rat)
            var index = (birthYear - 1900) % 12;
            if (index < 0)
                index += 12; // handle negative years

            var sign = ZodiacAnimals[index];

            return Ok(new
            {
                BirthYear = birthYear,
                ChineseZodiac = sign
            });
        }
    }
}
