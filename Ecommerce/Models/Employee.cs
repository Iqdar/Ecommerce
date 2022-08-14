using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ecommerce.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [DisplayName("Joining Date")]
        public DateTime DateJoined { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime BirthDate { get; set; }

    }
}
