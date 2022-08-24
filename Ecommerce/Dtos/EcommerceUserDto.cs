using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class EcommerceUserDto
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
