using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Auriqo_Web_Api_Backend.Models.DomainModels;

namespace Auriqo_Web_Api_Backend.Models.JunctionModels;

public class CartProduct
{
    [Key]
    public  Guid CartProductId { get; set; } = Guid.NewGuid();   // PRIMARY KEY
    public required Guid CartId { get; set; } // FOREIGN KEY
    [ForeignKey("CartId")]

    [JsonIgnore]
    public Cart? Cart { get; set; } // NAVIGATION PROPERTY
    public required Guid ProductId { get; set; } // FOREIGN KEY
    [ForeignKey("ProductId")]

    [JsonIgnore]
    public Product? Product { get; set; } // NAVIGATION PROPERTY

    public required decimal ProductPrice { get; set; }
    public required int Quantity { get; set; } = 1;
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Weight { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

}