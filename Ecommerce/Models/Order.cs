using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Ecommerce.Areas.Identity.Data;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Bill { get; set; }
        [DisplayName("Order Status")]
        public string Status { get; set; }
        [DisplayName("Payment Status")]
        public bool PaymentStatus { get; set; }
        public EcommerceUser User { get; set; }
        public string UserId { get; set; }
        public ICollection<Cart> Carts { get; set; }

    }
}
