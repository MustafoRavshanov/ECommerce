using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace ECommerce.Infrastructure.Data;

public class ApplicationDbContext(IConfiguration configuration):DbContext
{
    private readonly string? _connection = configuration.GetConnectionString("DefaultConnection");

    public DbSet<User>Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<FileData> FileDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.Id);

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.Permission });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<RolePermission>()
            .HasKey(x => new { x.RoleId, x.Permission });

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "SuperAdmin" },
            new Role { Id = 2, Name = "Customer" });

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, Permission = Permission.UsersDelete },
            new RolePermission { RoleId = 1, Permission = Permission.UsersView },
            new RolePermission { RoleId = 1, Permission = Permission.UsersUpdate },
            new RolePermission { RoleId = 1, Permission = Permission.OrdersCancel },
            new RolePermission { RoleId = 1, Permission = Permission.OrdersEdit },
            new RolePermission { RoleId = 1, Permission = Permission.OrdersView },
            new RolePermission { RoleId = 1, Permission = Permission.ProductsDelete },
            new RolePermission { RoleId = 1, Permission = Permission.ProductsView },
            new RolePermission { RoleId = 1, Permission = Permission.ProductsCreate },
            new RolePermission { RoleId = 1, Permission = Permission.ProductsEdit },
            new RolePermission { RoleId = 1, Permission = Permission.CategoriesManage },
            new RolePermission { RoleId = 1, Permission = Permission.AddressesManage },
            new RolePermission { RoleId = 1, Permission = Permission.RolesManage },
            new RolePermission { RoleId = 2, Permission = Permission.UsersUpdate },
            new RolePermission { RoleId = 2, Permission = Permission.OrdersEdit }
            );

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            PhoneNumber = "500016252",
            PasswordHash = BC.HashPassword("mustafo2006"),
            FirstName = "Mustafo",
            LastName = "Ravshanov",
            IsActive = true,
            RoleId = 1,
            CreatedAt= DateTime.UtcNow
        });
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseNpgsql(_connection);
    }
}
