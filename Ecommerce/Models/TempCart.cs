using System.ComponentModel.DataAnnotations;
using Ecommerce.Areas.Identity.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class TempCart
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
        public Inventory Inventory { get; set; }
        [DisplayName("ItemId")]
        public int InventoryId { get; set; }
        public EcommerceUser User { get; set; }
        public string UserId { get; set; }
    }
}
