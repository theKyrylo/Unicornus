using Microsoft.EntityFrameworkCore;
using Unicornus.Core.Models;

namespace Unicornus.Infrastructure.Data
{
    public class UnicornDbContext : DbContext
    {
        public UnicornDbContext(DbContextOptions<UnicornDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<CompanyInvestor> CompanyInvestors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("companies");
                entity.HasKey(e => e.CompanyId);
                entity.Property(e => e.CompanyId).HasColumnName("company_id");
                entity.Property(e => e.CompanyName).HasColumnName("company_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.YearFounded).HasColumnName("year_founded");
                entity.Property(e => e.ValuationInBillions).HasColumnName("valuation_in_billions").HasColumnType("decimal(15,2)");
                entity.Property(e => e.DateJoinedUnicorn).HasColumnName("date_joined_unicorn");
                entity.Property(e => e.FundingAmount).HasColumnName("funding_amount").HasColumnType("decimal(20,2)");
                entity.Property(e => e.FundingUnit).HasColumnName("funding_unit").HasMaxLength(1);
                entity.Property(e => e.LocationId).HasColumnName("location_id");
                entity.Property(e => e.IndustryId).HasColumnName("industry_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Industry)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.IndustryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CompanyName).IsUnique();
            });
            
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations");
                entity.HasKey(e => e.LocationId);
                entity.Property(e => e.LocationId).HasColumnName("location_id");
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.ContinentId).HasColumnName("continent_id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Continent)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.ContinentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.City, e.CountryId }).IsUnique();
            });
            
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");
                entity.HasKey(e => e.CountryId);
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.CountryName).HasColumnName("country_name").HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.CountryName).IsUnique();
            });
            
            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("continents");
                entity.HasKey(e => e.ContinentId);
                entity.Property(e => e.ContinentId).HasColumnName("continent_id");
                entity.Property(e => e.ContinentName).HasColumnName("continent_name").HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.ContinentName).IsUnique();
            });
            
            modelBuilder.Entity<Industry>(entity =>
            {
                entity.ToTable("industries");
                entity.HasKey(e => e.IndustryId);
                entity.Property(e => e.IndustryId).HasColumnName("industry_id");
                entity.Property(e => e.IndustryName).HasColumnName("industry_name").HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.IndustryName).IsUnique();
            });
            
            modelBuilder.Entity<Investor>(entity =>
            {
                entity.ToTable("investors");
                entity.HasKey(e => e.InvestorId);
                entity.Property(e => e.InvestorId).HasColumnName("investor_id");
                entity.Property(e => e.InvestorName).HasColumnName("investor_name").HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.InvestorName).IsUnique();
            });
            
            modelBuilder.Entity<CompanyInvestor>(entity =>
            {
                entity.ToTable("company_investors");
                entity.HasKey(e => new { e.CompanyId, e.InvestorId });
                entity.Property(e => e.CompanyId).HasColumnName("company_id");
                entity.Property(e => e.InvestorId).HasColumnName("investor_id");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyInvestors)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Investor)
                    .WithMany(p => p.CompanyInvestors)
                    .HasForeignKey(d => d.InvestorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}