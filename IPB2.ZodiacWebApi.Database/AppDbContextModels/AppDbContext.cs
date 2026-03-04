using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IPB2.ZodiacWebApi.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Trait> Traits { get; set; }

    public virtual DbSet<ZodiacSign> ZodiacSigns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trait>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Traits__3214EC07110537CD");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.ZodiacSign).WithMany(p => p.Traits)
                .HasForeignKey(d => d.ZodiacSignId)
                .HasConstraintName("FK_Traits_ZodiacSigns");
        });

        modelBuilder.Entity<ZodiacSign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZodiacSi__3214EC0748497B6B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Dates).HasMaxLength(100);
            entity.Property(e => e.Element).HasMaxLength(50);
            entity.Property(e => e.ElementImageUrl).HasMaxLength(255);
            entity.Property(e => e.MyanmarMonth).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ZodiacSign2ImageUrl).HasMaxLength(255);
            entity.Property(e => e.ZodiacSignImageUrl).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
