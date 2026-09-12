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
    
    public DbSet<PurchaseBill> PurchaseBills { get; set; }

    public DbSet<PurchaseBillItem> PurchaseBillItems { get; set; }

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

        modelBuilder.Entity<PurchaseBill>(entity =>
        {
            entity.ToTable("Purchase_Bills");

            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<PurchaseBillItem>(entity =>
        {
            entity.ToTable("Purchase_Bill_Items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ItemName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LocationCode)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.BatchName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.StandardCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.StandardPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(x => x.TotalCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalSelling)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(x => x.PurchaseBill)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseBillId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}