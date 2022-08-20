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
            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Employee/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<Employee>();
                    readTast.Wait();
                    employee = readTast.Result;
                }
            }

            return View("EmployeeForm", employee);
        }

        [ValidateAntiForgeryToken]
        public IActionResult Save(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View("EmployeeForm", employee);
            }
            else
            {

                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7271/api/");

                    if(employee.Id == 0)
                    {
                        var postTask = client.PostAsJsonAsync<Employee>("Employee", employee);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        var putTask = client.PutAsJsonAsync<Employee>("Employee", employee);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {

                            return RedirectToAction("Index");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }

                /*if (employee.Id == 0)
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
                _context.SaveChanges();*/
            }

            return View("EmployeeForm", employee);
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
            //Employee employee = _context.Employees.Single(c => c.Id == id);

            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Employee/"+id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<Employee>();
                    readTast.Wait();
                    employee = readTast.Result;
                }
            }

            return View(employee);
        }

        public IActionResult Index()
        {
            //var employee = _context.Employees.ToList();

            IEnumerable<Employee> employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7271/api/");
                var responseTask = client.GetAsync("Employee/");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<IList<Employee>>();
                    readTast.Wait();
                    employee = readTast.Result;
                }
            }

            return View(employee);
        }
    }
}
