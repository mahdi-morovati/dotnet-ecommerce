using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagement.Infrastructure.EFCore;

public class DiscountContext : DbContext
{
    public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
}