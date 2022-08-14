using Ecommerce.Areas.Identity.Data;

namespace Ecommerce.Models.ViewModels
{
    public class OrderViewModel
    {

        public Order Orders { get; set; }
        public IEnumerable<EcommerceUser> User { get; set; }
    }
}
