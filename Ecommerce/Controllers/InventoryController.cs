using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;

namespace Ecommerce.Controllers
{
    public class InventoryController : Controller
    {
        private EcommerceContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public InventoryController(EcommerceContext db, IWebHostEnvironment webHostEnvironment)
        {
            _context = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Add()
        {
            Inventory inventory = new Inventory();
            return View("InventoryForm", inventory);
        }

        public ActionResult Edit(int id)
        {
            Inventory inventory = _context.Inventory.Single(c => c.Id == id);
            return View("InventoryForm", inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {

                return View("InventoryForm", inventory);


            }
            else
            {

                string fileName = null;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string uploadDir = Path.Combine(wwwRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + inventory.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    inventory.Image.CopyTo(fileStream);
                }

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
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index", "Inventory");
        }

        public ActionResult Details(int id)
        {
            Inventory inventory = _context.Inventory.Single(c => c.Id == id);
            return View(inventory);
        }

        public IActionResult Index()
        {
            var inventory = _context.Inventory.ToList();
            return View(inventory);
        }
    }
}
