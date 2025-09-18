using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auriqo_Web_Api_Backend.Models.JunctionModels;
using Auriqo_Web_Api_Backend.Types;

namespace Auriqo_Web_Api_Backend.Models.DomainModels;

public class Order
{
[Key]
public Guid OrderId { get; set; } = Guid.NewGuid();
public required Guid UserId { get; set; }  // FOREIGN KEY 
[ForeignKey("UserId")]

public User? Buyer { get; set; }  // NAVIGATION PROPERTY
public required Guid AddressId { get; set; }  // FOREIGN KEY 
[ForeignKey("AddressId")]

public Address? Address { get; set; }   // NAVIGATION PROPERTY
public required ICollection<OrderProduct> OrderProducts { get; set; } = [];

public required decimal TotalPrice { get; set; } = 0;
public  DateTime DateCreated { get; set; } = DateTime.UtcNow;
public  OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
public  PaymentMode PaymentMode { get; set; } = PaymentMode.None;
public  PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;
public  DateTime? ShippingDate { get; set; } =DateTime.UtcNow.AddDays(7);

}


