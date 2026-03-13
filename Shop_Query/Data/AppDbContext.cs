using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shop_Query.Data.Models;

namespace Shop_Query.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer Config Start : 
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().Property(c => c.Name)
                .IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Phone)
                .HasMaxLength(20);
            modelBuilder.Entity<Customer>().Property(c => c.Email)
                .HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Address)
                .HasMaxLength(200);
            // Customer Config End . 

            
            // Category Config Start :
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name)
                .IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(c => c.Description)
                .HasMaxLength(500);
            // Category Config End .

            // Product Config Start :
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Name)
                .IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Product>().Property(p => p.Description)
                .HasMaxLength(1000);
            modelBuilder.Entity<Product>().Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            // Product Config End .

            // Order Config Start :
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().Property(o => o.Status)
                .IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.OrderDate)
                .IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.PaymentMethod)
                .IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.CustomerId)
                .IsRequired();
            // Order Config End .

            // OrderItems Config Start :
            modelBuilder.Entity<OrderItems>().HasKey(oi => oi.Id);
            modelBuilder.Entity<OrderItems>().Property(oi => oi.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderItems>().Property(oi => oi.ProductId)
                .IsRequired();
            modelBuilder.Entity<OrderItems>().Property(oi => oi.Quantity)
                .IsRequired();
            modelBuilder.Entity<OrderItems>().Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            // OrderItems Config End .

            // Relationships Config Start :
            // Relationship between Customer and Order Start :

            // Customer - Order (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(c => c.Customer)
                .WithMany(o => o.orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
             
             
            // Category - Product (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - OrderItems (one-to-many)
            modelBuilder.Entity<OrderItems>()
                .HasOne( o => o.Order)
                .WithMany(oi => oi.orderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - OrderItems (one-to-many)
            modelBuilder.Entity<OrderItems>()
                .HasOne(p => p.Product)
                .WithMany(oi => oi.orderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship Config End .

        }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
