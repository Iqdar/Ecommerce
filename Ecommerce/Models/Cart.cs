using System.ComponentModel.DataAnnotations;
using Ecommerce.Areas.Identity.Data;
using System.ComponentModel;

namespace Ecommerce.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
        [DisplayName("Item Id")]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string UserId { get; set; }
        public EcommerceUser User { get; set; }

    }
}
