using System;
using Microsoft.EntityFrameworkCore;
// using Auriqo-Web-Api-Backend.Models.DomainModels;
// using Auriqo-Web-Api-Backend.Models.JunctionModels;


namespace Auriqo-Web-Api-Backend.Data;

public class SqlDbContext : DbContext
{


    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }


    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Address>()
        .HasOne(a => a.Buyer)
        .WithMany(b => b.Addresses)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Cascade); //  cascade delete


        modelBuilder.Entity<Cart>()
        .HasOne(c => c.Buyer)
        .WithOne(b => b.Cart)
        .HasForeignKey<Cart>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade); // cascade delete


        modelBuilder.Entity<Order>()
        .HasOne(o => o.Buyer)      // every order belongs to the single user
        .WithMany(b => b.Orders)   // user can have many orders
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete



        modelBuilder.Entity<Order>()
        .HasOne(o => o.Address)
        .WithMany(a => a.Orders)
        .HasForeignKey(o => o.AddressId)
        .OnDelete(DeleteBehavior.Restrict);





        modelBuilder.Entity<CartProduct>()
        .HasKey(cp => new { cp.CartId, cp.ProductId }); // composite key


        modelBuilder.Entity<CartProduct>()
        .HasOne(cp => cp.Cart)
        .WithMany(c => c.CartProducts) // cart can have many products
        .HasForeignKey(cp => cp.CartId)
        .OnDelete(DeleteBehavior.Cascade); // Prevent cascade delete


        modelBuilder.Entity<CartProduct>()
        .HasOne(cp => cp.Product)       // every carTproduct is having product with many carts having the same product
        .WithMany(p => p.ProductInCarts)
        .HasForeignKey(cp => cp.ProductId)
        .OnDelete(DeleteBehavior.Cascade); // Prevent cascade delete




        modelBuilder.Entity<OrderProduct>()
        .HasKey(op => new { op.OrderId, op.ProductId }); // composite key


        modelBuilder.Entity<OrderProduct>()
        .HasOne(op => op.Order)
        .WithMany(o => o.OrderProducts)
        .HasForeignKey(op => op.OrderId)
        .OnDelete(DeleteBehavior.Cascade); //  cascade delete



        modelBuilder.Entity<OrderProduct>()
        .HasOne(op => op.Product)       // every orderproduct is having product with many orders having the same product
        .WithMany(p => p.ProductInOrders)
        .HasForeignKey(op => op.ProductId)
        .OnDelete(DeleteBehavior.Cascade); //  cascade delete
    }


}