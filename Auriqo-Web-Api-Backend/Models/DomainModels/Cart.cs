using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auriqo_Web_Api_Backend.Models.JunctionModels;
using Newtonsoft.Json;

namespace Auriqo_Web_Api_Backend.Models.DomainModels;

public class Cart
{
    [Key]
    public Guid CartId { get; set; } = Guid.NewGuid();
    public required Guid UserId { get; set; }  // FOREIGN KEY
    [ForeignKey("UserId")]

    [JsonIgnore]
    public User? Buyer { get; set; }  // NAVIGATION PROPERTY
    public ICollection<CartProduct> CartProducts { get; set; } = []; 
    

    // public ICollection<Product> Products { get; set; } = [];

    public required decimal CartTotal { get; set; } = 0; 

}