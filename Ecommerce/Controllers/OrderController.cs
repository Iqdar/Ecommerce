using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Areas.Identity.Data;
using System.Linq;
using Ecommerce.Areas.Identity.Pages.Account;
using Ecommerce.Models;
using Ecommerce.Dtos;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.ViewModels;

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
                /*
                IEnumerable<Order> orders = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");
                    var responseTask = client.GetAsync("Order/" + _userManager.GetUserId(User));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTast = result.Content.ReadAsAsync<IList<Order>>();
                        readTast.Wait();
                        orders = readTast.Result;
                    }
                }

                var orders = _context.Orders.Where(c => c.UserId == _userManager.GetUserId(User)).Include(c => c.User).ToList();*/
                return View();
            }
            return Redirect("/Identity/Account/Login/");
        }

        public ActionResult Details(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                IEnumerable<Cart> Cart = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");
                    var responseTask = client.GetAsync("Cart/"+id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTast = result.Content.ReadAsAsync<IList<Cart>>();
                        readTast.Wait();
                        Cart = readTast.Result;
                    }
                }

                //var orderCart = _context.Cart.Where(c => c.OrderId == id).Include(c => c.User).Include(c => c.Inventory).Include(c => c.Order).ToList();
                return View(Cart);
            }
            return Redirect("/Identity/Account/Login/");
        }

        public ActionResult Cart()
        {
            if (_signInManager.IsSignedIn(User))
            {
                /*var cart = _context.TempCart.Include(c => c.Inventory).Include(c => c.User).Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
                return View(cart);*/
                IEnumerable<TempCart> tempCart = null;
                using (var client = new HttpClient())
                {
                    string user = _userManager.GetUserId(User);
                    Console.WriteLine(user);
                    client.BaseAddress = new Uri("https://localhost:7271/api/");
                    var responseTask = client.GetAsync("TempCart/"+_userManager.GetUserId(User));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTast = result.Content.ReadAsAsync<IList<TempCart>>();
                        readTast.Wait();
                        tempCart = readTast.Result;
                    }
                }
                return View(tempCart);
            }
            return Redirect("/Identity/Account/Login/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCart(InventoryViewModel order)
        {
            if (_signInManager.IsSignedIn(User))
            {
                TempCart cart = new TempCart();
                cart.InventoryId = order.Id;
                cart.UserId = _userManager.GetUserId(User);
                cart.Quantity = order.OrderQuantity;
                cart.Cost = order.Price * order.OrderQuantity;
            
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");

                    var postTask = client.PostAsJsonAsync<TempCart>("TempCart", cart);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Cart");
                    }
                    
                }

                return RedirectToAction("Details", "Inventory", order.Id);
            }
            return Redirect("/Identity/Account/Login/");

        }
        /*
        public IActionResult DeleteCart(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");

                    //HTTP DELETE
                    var deleteTask = client.DeleteAsync("TempCart/" + id.ToString());
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");
                    }
                }

            }
            return Redirect("/Identity/Account/Login/");
        }

        public IActionResult AddOrder()
        {
            if (_signInManager.IsSignedIn(User))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");
                    InventoryViewModel inventoryViewModel = new InventoryViewModel();
                    inventoryViewModel.UserId = _userManager.GetUserId(User);
                    var postTask = client.PostAsJsonAsync<InventoryViewModel>("Order", inventoryViewModel);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
            return Redirect("/Identity/Account/Login/");
        }
        */
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
