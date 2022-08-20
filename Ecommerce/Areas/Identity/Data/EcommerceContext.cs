using Ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecommerce.Models.ViewModels;

namespace Ecommerce.Areas.Identity.Data;
public class EcommerceContext : IdentityDbContext<EcommerceUser>
{
    public EcommerceContext(DbContextOptions<EcommerceContext> options)
        : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<TempCart> TempCart { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new EcommerceUserEntityConfiguration());
    }

    public DbSet<Ecommerce.Models.ViewModels.InventoryViewModel>? InventoryViewModel { get; set; }
}

public class EcommerceUserEntityConfiguration : IEntityTypeConfiguration<EcommerceUser>
{
    public void Configure(EntityTypeBuilder<EcommerceUser> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.Address);
        builder.Property(u => u.BirthDate);
    }
}