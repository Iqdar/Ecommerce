using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Areas.Identity.Data;
using Ecommerce.Models;
using Ecommerce.Dtos;


namespace Ecommerce.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly EcommerceContext _context;

        public EmployeeController(EcommerceContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Example
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var emp = _context.Employees.ToList();
            if (emp == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<Employee>, List<EmployeeDto>>(emp);
        }

        // GET: api/Example/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return _mapper.Map<Employee, EmployeeDto>(employee);
        }

        // PUT: api/Example/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEmployee(EmployeeDto employeeDto)
        {

            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employeeDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Example
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeDto employeeDto)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'EcommerceContext.Employees'  is null.");
            }
            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employeeDto.Id }, employeeDto);
        }

        // DELETE: api/Example/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
