using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class OrderDto
    {
        [Key]
        public int Id { get; set; }
        public int Bill { get; set; }
        public string Status { get; set; }
        public bool PaymentStatus { get; set; }
        public EcommerceUserDto User { get; set; }
        public string UserId { get; set; }

    }
}
