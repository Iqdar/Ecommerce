using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Controllers.Api;
using Ecommerce.Areas.Identity.Data;

namespace Ecommerce.Controllers
{
    public class EmployeeController : Controller
    {
        private EcommerceContext _context;

        public EmployeeController(EcommerceContext db)
        {
            _context = db;
        }

        public ActionResult Add()
        {
            Employee employee = new Employee();
            return View("EmployeeForm", employee);
        }

        public ActionResult Edit(int id)
        {
            Employee employee = _context.Employees.Single(c => c.Id == id);
            return View("EmployeeForm", employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View("EmployeeForm", employee);
            }
            else
            {
                if (employee.Id == 0)
                {

                    _context.Employees.Add(employee);
                }
                else
                {
                    var employeeInDb = _context.Employees.Single((System.Linq.Expressions.Expression<Func<Employee, bool>>)(c => c.Id == employee.Id));

                    employeeInDb.Name = employee.Name;
                    employeeInDb.Address = employee.Address;
                    employeeInDb.BirthDate = employee.BirthDate;
                    employeeInDb.DateJoined =  employee.DateJoined;
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Employee");
        }

        public ActionResult Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }

        public ActionResult Details(int id)
        {
            Employee employee = _context.Employees.Single(c => c.Id == id);
            return View(employee);
        }

        public IActionResult Index()
        {
            var employee = _context.Employees.ToList();
            return View(employee);
        }
    }
}
