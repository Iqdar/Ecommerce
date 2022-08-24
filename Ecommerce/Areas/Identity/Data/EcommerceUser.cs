using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Areas.Identity.Data;

// Add profile data for application users by adding properties to the EcommerceUser class
public class EcommerceUser : IdentityUser
{
    public string Name { get; set; }
    //public string Password { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<TempCart> TempCarts { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Cart> Carts { get; set; }

}

