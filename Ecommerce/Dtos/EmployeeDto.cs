using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class EmployeeDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
