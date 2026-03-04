// See https://aka.ms/new-console-template for more information
//using IPB2.ZodiacConsole;
//using Newtonsoft.Json;

//Console.WriteLine("Hello, World!");

//var json = File.ReadAllText("Zodiac.json");

//var item = JsonConvert.DeserializeObject<Zodiacs>(json);

//Console.ReadLine();

using IPB2.ZodiacConsole;
using IPB2.ZodiacWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

optionsBuilder.UseSqlServer(
    "Server=.;Database=ZodiacDb;User ID=sa;Password=12345;TrustServerCertificate=True;");

using var context = new AppDbContext(optionsBuilder.Options);

// Prevent duplicate insert
if (context.ZodiacSigns.Any())
{
    Console.WriteLine("Data already exists!");
    return;
}

// Read JSON file
var json = File.ReadAllText("Zodiac.json");
var data = JsonConvert.DeserializeObject<Zodiacs>(json);

foreach (var zodiac in data.ZodiacSignsDetail)
{
    var zodiacEntity = new ZodiacSign
    {
        Id = zodiac.Id,
        Name = zodiac.Name,
        MyanmarMonth = zodiac.MyanmarMonth,
        ZodiacSignImageUrl = zodiac.ZodiacSignImageUrl,
        ZodiacSign2ImageUrl = zodiac.ZodiacSign2ImageUrl,
        Dates = zodiac.Dates,
        Element = zodiac.Element,
        ElementImageUrl = zodiac.ElementImageUrl,
        LifePurpose = zodiac.LifePurpose,
        Loyal = zodiac.Loyal,
        RepresentativeFlower = zodiac.RepresentativeFlower,
        Angry = zodiac.Angry,
        Character = zodiac.Character,
        PrettyFeatures = zodiac.PrettyFeatures
    };

    context.ZodiacSigns.Add(zodiacEntity);
    context.SaveChanges(); 

    foreach (var trait in zodiac.Traits)
    {
        var traitEntity = new Trait
        {
            ZodiacSignId = zodiacEntity.Id,
            Name = trait.name,
            Percentage = trait.percentage
        };

        context.Traits.Add(traitEntity);
    }

    context.SaveChanges();
}

Console.WriteLine("✅ Data Imported Successfully!");
Console.ReadLine();