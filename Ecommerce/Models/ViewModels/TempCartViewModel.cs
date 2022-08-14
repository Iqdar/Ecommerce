namespace Ecommerce.Models.ViewModels
{
    public class TempCartViewModel
    {

        public TempCart TempCarts { get; set; }
        public IEnumerable<Inventory> Inventory { get; set; }
    }
}
