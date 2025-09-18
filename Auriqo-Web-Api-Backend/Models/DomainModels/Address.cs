using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auriqo_Web_Api_Backend.Models.DomainModels;

public class Address
{

[Key]
public Guid AddressId {get; set;} = Guid.NewGuid();
public required string Name {get; set;}
public required string City {get; set;}
public required string District {get; set;}
public required string State {get; set;}
public required string Country {get; set;}
public required string Pincode {get; set;}
public required string Phone {get; set;}
public required string Email {get; set;}

public required Guid UserId {get; set;} //FOREIGN KEY

[ForeignKey("UserId")]
public User? Buyer {get; set;} //NAVIGATION PROPERTY
public ICollection<Order> Orders {get; set;} = []; //NAVIGATION PROPERTY 

}
