using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auriqo_Web_Api_Backend.Models.DomainModels;

namespace Auriqo_Web_Api_Backend.Models.JunctionModels;

public class OrderProduct
{

    [Key]

    public Guid OrderProductId { get; set; } = Guid.NewGuid();   // PRIMARY KEY
    public required Guid OrderId { get; set; } // FOREIGN KEY
    [ForeignKey("OrderId")]

    public Order? Order { get; set; } // NAVIGATION PROPERTY

    public required Guid ProductId { get; set; } // FOREIGN KEY
    [ForeignKey("ProductId")]

    public Product? Product { get; set; } // NAVIGATION PROPERTY
    public required int Quantity { get; set; } = 1;
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Weight { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

}