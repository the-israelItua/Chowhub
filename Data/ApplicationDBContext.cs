using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Data
{
    public class ApplicationDBContext : IdentityDbContext(User)
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions){

        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {       
                base.OnModelCreating(modelBuilder);
                // User-Cart Relationship
                modelBuilder.Entity<Cart>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Carts)
                    .HasForeignKey(c => c.UserId);

                // Restaurant-Cart Relationship
                modelBuilder.Entity<Cart>()
                    .HasOne(c => c.Restaurant)
                    .WithMany(r => r.Carts)
                    .HasForeignKey(c => c.RestaurantId);

                // Cart-CartItem Relationship
                modelBuilder.Entity<CartItem>()
                    .HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId);

                // CartItem-Product Relationship
                modelBuilder.Entity<CartItem>()
                    .HasOne(ci => ci.Product)
                    .WithMany()
                    .HasForeignKey(ci => ci.ProductId);

                // Restaurant-Product Relationship
                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Restaurant)
                    .WithMany(r => r.Products)
                    .HasForeignKey(p => p.RestaurantId);

                // User-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);

                // Restaurant-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Restaurant)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.RestaurantId);

                // Order-OrderItem Relationship
                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId);

                // OrderItem-Product Relationship
                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId);

                // Driver-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Driver)
                    .WithMany(d => d.Orders)
                    .HasForeignKey(o => o.DriverId);
            }

     }
}