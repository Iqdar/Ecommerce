using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Dtos
{
    public class InventoryDto
    {

        [Key]
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int StockRemaining { get; set; }
        public int Price { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [NotMapped]
        public int OrderQuantity { get; set; }
    }
}
