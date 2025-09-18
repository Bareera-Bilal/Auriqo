using System;
using Auriqo_Web_Api_Backend.Models.DomainModels;
using Auriqo_Web_Api_Backend.Models.JunctionModels;
using Microsoft.EntityFrameworkCore;


namespace Auriqo_Web_Api_Backend;

public class SqlDbContext : DbContext
{


    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

// Domain Models 
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }


// Junction Models
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Address>()
        .HasOne(a => a.Buyer)
        .WithMany(b => b.Addresses)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Cascade); // CASCADE DELETE


        modelBuilder.Entity<Cart>()
        .HasOne(c => c.Buyer)
        .WithOne(b => b.Cart)
        .HasForeignKey<Cart>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade); // CASCADE DELETE


        modelBuilder.Entity<Order>()
        .HasOne(o => o.Buyer)      // EVERY ORDER BELONGS TO SINGLE USER
        .WithMany(b => b.Orders)   // USER CAN HAVE MORE THAN 1 ORDERS
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict); // PREVENT CASCADE DELETE


        modelBuilder.Entity<Order>()
        .HasOne(o => o.Address)
        .WithMany(a => a.Orders)
        .HasForeignKey(o => o.AddressId)
        .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<CartProduct>()
        .HasKey(cp => new { cp.CartId, cp.ProductId }); //COMPOSITE KEY


        modelBuilder.Entity<CartProduct>()
        .HasOne(cp => cp.Cart)
        .WithMany(c => c.CartProducts) // CART CAN HAVE MORE THAN 1 PRODUCTS
        .HasForeignKey(cp => cp.CartId)
        .OnDelete(DeleteBehavior.Cascade); // PREVENT CASCADE DELETE


        // modelBuilder.Entity<CartProduct>()
        // .HasOne(cp => cp.Product)       // EVERY CART PRODUCT IS HAVING PRODUCT WITH MANY CARTS HAVING THE SAME PRODUCTS
        // .WithMany(p => p.ProductInCarts)
        // .HasForeignKey(cp => cp.ProductId)
        // .OnDelete(DeleteBehavior.Cascade); // PREVENT CASCADE DELETE


        modelBuilder.Entity<OrderProduct>()
        .HasKey(op => new { op.OrderId, op.ProductId }); // COMPOSITE KEY

        modelBuilder.Entity<OrderProduct>()
        .HasOne(op => op.Order)
        .WithMany(o => o.OrderProducts)
        .HasForeignKey(op => op.OrderId)
        .OnDelete(DeleteBehavior.Cascade); // CASCADE DELETE 


    //     modelBuilder.Entity<OrderProduct>()
    //     .HasOne(op => op.Product)       // EVERY PRODUCT IS HAVING PRODUCT WITH MANY ORDERS HAVING THE SAME PRODUCT
    //     .WithMany(p => p.ProductInOrders)
    //     .HasForeignKey(op => op.ProductId)
    //     .OnDelete(DeleteBehavior.Cascade); // CASACADE DELETE
    // }

    }
}