using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<LocationDetail> LocationDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LocationDetail>(entity =>
        {
            entity.ToTable("Location_Details");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.LocationCode)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LocationName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Address)
                .HasMaxLength(500);

            entity.Property(x => x.Phone)
                .HasMaxLength(50);

            entity.Property(x => x.CompanyCode)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => new
            {
                x.CompanyCode,
                x.LocationCode
            })
            .IsUnique();
        });
    }
}