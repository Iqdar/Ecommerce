using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Areas.Identity.Data;
using System.Linq;
using Ecommerce.Areas.Identity.Pages.Account;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly SignInManager<EcommerceUser> _signInManager;
        private readonly UserManager<EcommerceUser> _userManager;
        private EcommerceContext _context;


        public OrderController(UserManager<EcommerceUser> userManager,
            SignInManager<EcommerceUser> signInManager,
            EcommerceContext db)
        {
            _context = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Order
        public ActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var orders = _context.Orders.Where(c => c.UserId == _userManager.GetUserId(User)).Include(c => c.User).ToList();
                return View(orders);
            }
            return Redirect("/Identity/Account/Login/");
        }

        public ActionResult Details(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var orderCart = _context.Cart.Where(c => c.OrderId == id).Include(c => c.User).Include(c => c.Inventory).Include(c => c.Order).ToList();
                return View(orderCart);
            }
            return Redirect("/Identity/Account/Login/");
        }

        public ActionResult Cart()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var cart = _context.TempCart.Include(c => c.Inventory).Include(c => c.User).Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
                return View(cart);
            }
            return Redirect("/Identity/Account/Login/");
        }

        public ActionResult AddCart(Inventory order)
        {

            var inventory = _context.Inventory.Single(c => c.Id == order.Id);
            int stock = inventory.StockRemaining;
            if (_signInManager.IsSignedIn(User) && order.Id != 0 && order.Id != null && stock>= order.OrderQuantity)
            {

                inventory.StockRemaining = inventory.StockRemaining - order.OrderQuantity;

                int itemId = order.Id;
                int pricePerUnit = inventory.Price;
                int totalPrice = pricePerUnit * order.OrderQuantity;
                string userId = _userManager.GetUserId(User);

                TempCart cart = new TempCart();
                cart.InventoryId = itemId;
                cart.Cost = totalPrice;
                cart.Quantity = order.OrderQuantity;
                cart.UserId = userId;

                _context.TempCart.Add(cart);
                _context.SaveChanges();

                return RedirectToAction("Cart", "Order");
            }
            return Redirect("/Identity/Account/Login/");

        }

        public IActionResult DeleteCart(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                TempCart tempCart = _context.TempCart.Find(id);
                if (tempCart != null)
                {
                    var inventory = _context.Inventory.Single(c => c.Id == tempCart.InventoryId);
                    inventory.StockRemaining = inventory.StockRemaining + tempCart.Quantity;
                    _context.TempCart.Remove(tempCart);
                    _context.SaveChanges();
                    return RedirectToAction("Cart", "Order");
                }
                else
                {
                    return NotFound();
                }
            }
            return Redirect("/Identity/Account/Login/");
        }

        public IActionResult AddOrder()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var tempCart = _context.TempCart.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
                if (tempCart != null)
                {
                    int orderTotal = 0;
                    for (int i = 0; i < tempCart.Count; i++)
                    {
                        orderTotal = orderTotal + tempCart[i].Cost;
                    }

                    Order order = new Order();
                    order.UserId = _userManager.GetUserId(User);
                    order.Status = "Pending";
                    order.Bill = orderTotal;
                    order.PaymentStatus = false;

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    
                    int orderId = _context.Orders.Where(c => c.UserId == _userManager.GetUserId(User)).OrderByDescending(c => c.Id).FirstOrDefault().Id;

                    Cart cart;

                    for (int i = 0; i < tempCart.Count; i++)
                    {

                        cart = new Cart();
                        cart.OrderId = orderId;
                        cart.UserId = _userManager.GetUserId(User);
                        cart.Quantity = tempCart[i].Quantity;
                        cart.Cost = tempCart[i].Cost;
                        cart.InventoryId = tempCart[i].InventoryId;

                        _context.Cart.Add(cart);
                        _context.TempCart.Remove(tempCart[i]);
                        _context.SaveChanges();
                        return View("Cart");
                    }
                }
            }
            return Redirect("/Identity/Account/Login/");
        }

        public IActionResult UpdateOrder(int id, string orderStatus, bool paymentStatus) {
            var order = _context.Orders.Single(c => c.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                order.PaymentStatus = paymentStatus;
                order.Status = orderStatus;

                _context.SaveChanges();
            }
            return View();
        }
    }
}
