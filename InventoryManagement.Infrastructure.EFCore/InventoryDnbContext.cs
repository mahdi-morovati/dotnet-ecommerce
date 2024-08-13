using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.EFCore;

public class InventoryDnbContext : DbContext
{
    public DbSet<Inventory> Inventory { get; set; }

    protected InventoryDnbContext(DbContextOptions<InventoryDnbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(InventoryMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}