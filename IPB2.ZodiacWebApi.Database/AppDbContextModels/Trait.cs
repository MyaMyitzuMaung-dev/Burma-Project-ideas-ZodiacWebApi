using System;
using System.Collections.Generic;

namespace IPB2.ZodiacWebApi.Database.AppDbContextModels;

public partial class Trait
{
    public int Id { get; set; }

    public int ZodiacSignId { get; set; }

    public string? Name { get; set; }

    public int? Percentage { get; set; }

    public virtual ZodiacSign ZodiacSign { get; set; } = null!;
}
