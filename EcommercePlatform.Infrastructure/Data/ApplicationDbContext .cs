// ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EcommercePlatform.Core.Entities;

namespace EcommercePlatform.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<StoreSettings> StoreSettings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Store Configuration
            builder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.Domain).IsUnique();

                entity.HasOne(e => e.Owner)
                    .WithMany(u => u.Stores)
                    .HasForeignKey(e => e.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubscriptionPlan)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(e => e.SubscriptionPlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SubscriptionPlan Configuration
            builder.Entity<SubscriptionPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MonthlyPrice).HasPrecision(18, 2);
                entity.Property(e => e.YearlyPrice).HasPrecision(18, 2);
            });

            // StoreSettings Configuration
            builder.Entity<StoreSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Store)
                    .WithOne(s => s.Settings)
                    .HasForeignKey<StoreSettings>(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ShippingFee).HasPrecision(18, 2);
                entity.Property(e => e.FreeShippingThreshold).HasPrecision(18, 2);
            });

            // Product Configuration
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.CompareAtPrice).HasPrecision(18, 2);
                entity.HasIndex(e => new { e.StoreId, e.Slug }).IsUnique();

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict); // ✅ حل المشكلة هنا

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Category Configuration
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => new { e.StoreId, e.Slug }).IsUnique();

                entity.HasOne(e => e.Store)
                    .WithMany()
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict); // ✅ حل المشكلة هنا
            });

            // ProductImage Configuration
            builder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Order Configuration
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.SubTotal).HasPrecision(18, 2);
                entity.Property(e => e.ShippingFee).HasPrecision(18, 2);
                entity.Property(e => e.Total).HasPrecision(18, 2);

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict); // ✅ نمنع الكاسكيد
            });

            // OrderItem Configuration
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // Seed Data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // خطط الاشتراك
            builder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan
                {
                    Id = 1,
                    Name = "الخطة المجانية",
                    MonthlyPrice = 0,
                    YearlyPrice = 0,
                    MaxProducts = 10,
                    MaxOrders = 50,
                    HasCustomDomain = false,
                    HasAdvancedReports = false
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    Name = "الخطة الأساسية",
                    MonthlyPrice = 299,
                    YearlyPrice = 2990,
                    MaxProducts = 100,
                    MaxOrders = 500,
                    HasCustomDomain = false,
                    HasAdvancedReports = true
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    Name = "الخطة الاحترافية",
                    MonthlyPrice = 599,
                    YearlyPrice = 5990,
                    MaxProducts = 1000,
                    MaxOrders = -1, // غير محدود
                    HasCustomDomain = true,
                    HasAdvancedReports = true
                }
            );
        }
    }
}