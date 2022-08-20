using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Ecommerce.Models.ViewModels
{
    public class InventoryViewModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string ItemName { get; set; }
        public string Description { get; set; }
        [DisplayName("Stock")]
        public int StockRemaining { get; set; }
        public int Price { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? Image { get; set; }
        [NotMapped]
        [DisplayName("Quantity")]
        public int OrderQuantity { get; set; }
        [NotMapped]
        public string? UserId { get; set; }
    }
}
