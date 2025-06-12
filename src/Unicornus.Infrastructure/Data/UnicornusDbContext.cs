using Microsoft.EntityFrameworkCore;
using Unicornus.Core.Models;

namespace Unicornus.Infrastructure.Data;

public class UnicornusDbContext : DbContext
{
    public UnicornusDbContext(DbContextOptions<UnicornusDbContext> options) : base(options)
    {
    }

    public DbSet<Continent> Continents { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Industry> Industries { get; set; }
    public DbSet<Investor> Investors { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyInvestor> CompanyInvestors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.ContinentId);
            entity.Property(e => e.ContinentId).HasColumnName("continent_id");
            entity.Property(e => e.ContinentName)
                .HasColumnName("continent_name")
                .HasMaxLength(50)
                .IsRequired();
            entity.ToTable("continents");
        });
        
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId);
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasColumnName("country_name")
                .HasMaxLength(100)
                .IsRequired();
            entity.ToTable("countries");
        });
        
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId);
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.ContinentId).HasColumnName("continent_id");

            entity.HasOne(d => d.Country)
                .WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_country");

            entity.HasOne(d => d.Continent)
                .WithMany(p => p.Locations)
                .HasForeignKey(d => d.ContinentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_continent");

            entity.ToTable("locations");
        });
        
        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.IndustryId);
            entity.Property(e => e.IndustryId).HasColumnName("industry_id");
            entity.Property(e => e.IndustryName)
                .HasColumnName("industry_name")
                .HasMaxLength(100)
                .IsRequired();
            entity.ToTable("industries");
        });
        
        modelBuilder.Entity<Investor>(entity =>
        {
            entity.HasKey(e => e.InvestorId);
            entity.Property(e => e.InvestorId).HasColumnName("investor_id");
            entity.Property(e => e.InvestorName)
                .HasColumnName("investor_name")
                .HasMaxLength(100)
                .IsRequired();
            entity.ToTable("investors");
        });
        
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId);
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanyName)
                .HasColumnName("company_name")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.YearFounded).HasColumnName("year_founded");
            entity.Property(e => e.ValuationInBillions)
                .HasColumnName("valuation_in_billions")
                .HasPrecision(15, 2);
            entity.Property(e => e.DateJoinedUnicorn).HasColumnName("date_joined_unicorn");
            entity.Property(e => e.FundingAmount)
                .HasColumnName("funding_amount")
                .HasPrecision(20, 2);
            entity.Property(e => e.FundingUnit)
                .HasColumnName("funding_unit")
                .HasMaxLength(1);
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.IndustryId).HasColumnName("industry_id");

            entity.HasOne(d => d.Location)
                .WithMany(p => p.Companies)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_location");

            entity.HasOne(d => d.Industry)
                .WithMany(p => p.Companies)
                .HasForeignKey(d => d.IndustryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_industry");

            entity.ToTable("companies");
        });
        
        modelBuilder.Entity<CompanyInvestor>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.InvestorId });
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.InvestorId).HasColumnName("investor_id");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.CompanyInvestors)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_company");

            entity.HasOne(d => d.Investor)
                .WithMany(p => p.CompanyInvestors)
                .HasForeignKey(d => d.InvestorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_investor");

            entity.ToTable("company_investors");
        });

        base.OnModelCreating(modelBuilder);
    }
}