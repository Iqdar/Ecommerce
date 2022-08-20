using System.ComponentModel.DataAnnotations;
using Ecommerce.Models;
using Ecommerce.Areas.Identity.Data;

namespace Ecommerce.Dtos
{
    public class TempCartDto
    {

        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
        public InventoryDto Inventory { get; set; }
        public int InventoryId { get; set; }
        public EcommerceUserDto User { get; set; }
        public string UserId { get; set; }
    }
}
