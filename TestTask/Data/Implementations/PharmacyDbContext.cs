#nullable enable
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Implementations;

public class PharmacyDbContext : DbContext
{
    public PharmacyDbContext()
    {
    }

    public virtual DbSet<Medicine> Medicines { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.Medicines)
            .WithOne(m => m.Supplier)
            .HasForeignKey(m => m.SupplierId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Data/pharmacy.db");
    }
}
