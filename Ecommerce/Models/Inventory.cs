using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Inventory
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
        public IFormFile Image { get; set; }
        [NotMapped]
        [DisplayName("Quantity")]
        public int OrderQuantity { get; set; }
    }
}
