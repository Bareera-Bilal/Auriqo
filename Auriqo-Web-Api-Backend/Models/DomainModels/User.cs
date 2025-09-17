
using System.ComponentModel.DataAnnotations;
using  Auriqo_Web_Api_Backend.Types;


namespace Auriqo_Web_Api_Backend.Models.DomainModels;

public class User
{

    [Key]
    public  Guid UserId {get;set;} = Guid.NewGuid();
    public  string?  Username {get ;set;}
    public required string  Email {get ;set;}
    public required string  Password {get ;set;}
    public string?  ProfilePicUrl {get ;set;}
    public string? Phone {get; set ; }
    // public Role Role {get; set;} = Role.User;
    
    public ICollection<Address> Addresses { get; set; } = []; //  navigation property 
    public Cart?  Cart {get;set;}    // navigation property
    public ICollection<Order>? Orders {get;set;} =[]; // navigation property //  collection of orders placed by the user



}