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
    public class InventoryController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly EcommerceContext _context;

        public InventoryController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Example
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventory()
        {
            var inventory = _context.Inventory.ToList();
            if (inventory == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<Inventory>, List<InventoryDto>>(inventory);
        }

        // GET: api/Example/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> GetInventory(int id)
        {
            if (_context.Inventory == null)
            {
                return NotFound();
            }
            var inventory = await _context.Inventory.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return _mapper.Map<Inventory, InventoryDto>(inventory);
        }

        // PUT: api/Example/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, InventoryDto inventoryDto)
        {
            if (id != inventoryDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var inventory = _mapper.Map<InventoryDto, Inventory>(inventoryDto);
            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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
        public async Task<ActionResult<InventoryDto>> PostInventory(InventoryDto inventoryDto)
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'EcommerceContext.Employees'  is null.");
            }
            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var inventory = _mapper.Map<InventoryDto, Inventory>(inventoryDto);
            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventory", new { id = inventoryDto.Id }, inventoryDto);
        }

        // DELETE: api/Example/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            if (_context.Inventory == null)
            {
                return NotFound();
            }
            var employee = await _context.Inventory.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Inventory.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryExists(int id)
        {
            return (_context.Inventory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
