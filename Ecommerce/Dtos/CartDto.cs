using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class CartDto
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
        public InventoryDto Inventory { get; set; }
        public int InventoryId { get; set; }
        public OrderDto Order { get; set; }
        public int OrderId { get; set; }
        public EcommerceUserDto User { get; set; }
        public string UserId { get; set; }

    }
}
