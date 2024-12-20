using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions){

        }
        public DbSet<Customer> Customers { get; set; } = null!;
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

                modelBuilder.Entity<Customer>()
                    .HasOne(r => r.ApplicationUser) 
                    .WithOne()
                    .HasForeignKey<Customer>(r => r.ApplicationUserId) 
                    .OnDelete(DeleteBehavior.SetNull); 

                modelBuilder.Entity<Restaurant>()
                    .HasOne(r => r.ApplicationUser) 
                    .WithOne()
                    .HasForeignKey<Restaurant>(r => r.ApplicationUserId) 
                    .OnDelete(DeleteBehavior.SetNull); 

                modelBuilder.Entity<Driver>()
                    .HasOne(d => d.ApplicationUser) 
                    .WithOne() 
                    .HasForeignKey<Driver>(d => d.ApplicationUserId) 
                    .OnDelete(DeleteBehavior.SetNull);

          
                   modelBuilder.Entity<Cart>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Carts)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.SetNull); 

                // Restaurant-Cart Relationship
                modelBuilder.Entity<Cart>()
                    .HasOne(c => c.Restaurant)
                    .WithMany(r => r.Carts)
                    .HasForeignKey(c => c.RestaurantId)
              .OnDelete(DeleteBehavior.SetNull); 

                // Cart-CartItem Relationship
                modelBuilder.Entity<CartItem>()
                    .HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.SetNull); 

                // CartItem-Product Relationship
                modelBuilder.Entity<CartItem>()
                    .HasOne(ci => ci.Product)
                    .WithMany()
                    .HasForeignKey(ci => ci.ProductId)
                       .OnDelete(DeleteBehavior.SetNull); 

                // Restaurant-Product Relationship
                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Restaurant)
                    .WithMany(r => r.Products)
                    .HasForeignKey(p => p.RestaurantId)
                       .OnDelete(DeleteBehavior.SetNull); 

                // User-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.SetNull); 

                // Restaurant-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Restaurant)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.RestaurantId)
                   .OnDelete(DeleteBehavior.SetNull); 

                // Order-OrderItem Relationship
                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.SetNull); 

                // OrderItem-Product Relationship
                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.SetNull); 

                // Driver-Order Relationship
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Driver)
                    .WithMany(d => d.Orders)
                    .HasForeignKey(o => o.DriverId)
                    .OnDelete(DeleteBehavior.SetNull); 
            }

     }
}