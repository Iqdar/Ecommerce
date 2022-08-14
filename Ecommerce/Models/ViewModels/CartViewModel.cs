namespace Ecommerce.Models.ViewModels
{
    public class CartViewModel
    {
        public Cart Carts { get; set; }
        public IEnumerable<Inventory> Inventory { get; set; }
        public IEnumerable<Order> Order { get; set; }
    }
}
