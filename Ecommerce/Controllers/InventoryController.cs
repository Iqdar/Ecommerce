using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Dtos;
using Ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers
{
    public class InventoryController : Controller
    {
        private EcommerceContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        private UserManager<EcommerceUser> _userManager;

        public InventoryController(EcommerceContext db, IWebHostEnvironment webHostEnvironment, UserManager<EcommerceUser> userManager)
        {
            _context = db;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public ActionResult Add()
        {
            InventoryViewModel inventory = new InventoryViewModel();
            return View("InventoryForm", inventory);
        }

        public ActionResult Edit(int id)
        {
            Inventory inventory = null;
            InventoryViewModel ivm = new InventoryViewModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Inventory/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<Inventory>();
                    readTast.Wait();
                    inventory = readTast.Result;
                }
                ivm.Id = inventory.Id;
                ivm.ItemName = inventory.ItemName;
                ivm.Price = inventory.Price;
                ivm.Description = inventory.Description;
                ivm.ImageName = inventory.ImageName;
                ivm.StockRemaining = inventory.StockRemaining;
            }
            return View("InventoryForm", ivm);
        }

        [ValidateAntiForgeryToken]
        public IActionResult Save(InventoryViewModel inventory)
        {
            if (!ModelState.IsValid)
            {

                return View("InventoryForm", inventory);

            }
            else
            {
                Inventory iv = new Inventory();
                iv.Id = inventory.Id;
                iv.ItemName = inventory.ItemName;
                iv.Description = inventory.Description;
                iv.Price = inventory.Price;
                iv.StockRemaining = inventory.StockRemaining;
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");
                    
                    if (inventory.Id == 0)
                    {
                        if (inventory.Image != null)
                        {
                            string fileName = null;
                            string wwwRootPath = _webHostEnvironment.WebRootPath;
                            string uploadDir = Path.Combine(wwwRootPath, "Images");
                            fileName = inventory.Image.FileName.ToString();
                            string filePath = Path.Combine(uploadDir, fileName);
                            iv.ImageName = fileName;
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                inventory.Image.CopyTo(fileStream);
                            }

                        }
                        var postTask = client.PostAsJsonAsync<Inventory>("Inventory", iv);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {

                        if (inventory.Image != null)
                        {
                            string fileName = null;
                            string wwwRootPath = _webHostEnvironment.WebRootPath;
                            string uploadDir = Path.Combine(wwwRootPath, "Images");
                            fileName = inventory.Image.FileName.ToString();
                            string filePath = Path.Combine(uploadDir, fileName);
                            if (fileName != _context.Inventory.Find(inventory.Id).ImageName)
                            {
                                iv.ImageName = fileName;
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    inventory.Image.CopyTo(fileStream);
                                }
                            }
                        }
                        var putTask = client.PutAsJsonAsync<Inventory>("Inventory", iv);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");

                        }

                    }
                }
                /*
                    if (inventory.Id == 0)
                {
                    inventory.ImageName = fileName;
                    _context.Inventory.Add(inventory);
                }
                else
                {
                    var inventoryInDb = _context.Inventory.Single((System.Linq.Expressions.Expression<Func<Inventory, bool>>)(c => c.Id == inventory.Id));

                    inventoryInDb.ItemName = inventory.ItemName;
                    inventoryInDb.Description = inventory.Description;
                    inventoryInDb.StockRemaining = inventory.StockRemaining;
                    inventoryInDb.Price = inventory.Price;
                    inventoryInDb.Image = inventory.Image;
                    inventoryInDb.ImageName = fileName;
                }
                _context.SaveChanges();*/

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            

            return View("InventoryForm", inventory);
        }

        public ActionResult Details(int id)
        {
            Inventory inventory = null;
            InventoryViewModel ivm = new InventoryViewModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Inventory/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<Inventory>();
                    readTast.Wait();
                    inventory = readTast.Result;
                }
                ivm.Id = inventory.Id;
                ivm.ItemName = inventory.ItemName;
                ivm.Price = inventory.Price;
                ivm.Description = inventory.Description;
                ivm.ImageName = inventory.ImageName;
                ivm.StockRemaining = inventory.StockRemaining;
                ivm.UserId = _userManager.GetUserId(User);
            }
            return View(ivm);
        }

        public IActionResult Index()
        {
            IEnumerable<Inventory> inventory = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Inventory/");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<IList<Inventory>>();
                    readTast.Wait();
                    inventory = readTast.Result;
                }
            }
            return View(inventory);
        }
    }
}
