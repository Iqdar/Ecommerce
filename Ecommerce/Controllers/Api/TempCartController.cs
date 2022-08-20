using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Areas.Identity.Data;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;   
using Ecommerce.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EcommerceContext _context;
        private readonly SignInManager<EcommerceUser> _signInManager;
        private readonly UserManager<EcommerceUser> _userManager;

        public TempCartController(UserManager<EcommerceUser> userManager,
            SignInManager<EcommerceUser> signInManager, EcommerceContext context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Example
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TempCartDto>>> GetTempCarts(string userId)
        {
            var tempCart = _context.TempCart.Where(c => c.UserId == userId).Include(c => c.Inventory).Include(c => c.User).ToList();
            if (tempCart == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<TempCart>, List<TempCartDto>>(tempCart);
        }
        /*
        // GET: api/Example/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TempCartDto>> GetTempCart(int id)
        {
            if (_context.TempCart == null)
            {
                return NotFound();
            }
            var tempCart = await _context.TempCart.FindAsync(id);

            if (tempCart == null)
            {
                return NotFound();  
            }

            return _mapper.Map<TempCart, TempCartDto>(tempCart);
        }
        */
        // PUT: api/Example/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTempCart(int id, TempCartDto tempCartDto)
        {
            if (id != tempCartDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var tempCart = _mapper.Map<TempCartDto, TempCart>(tempCartDto);
            _context.Entry(tempCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TempCartExists(id))
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
        public async Task<ActionResult<TempCartDto>> PostTempCart(TempCartDto tempCart)
        {
            if (_context.TempCart == null)
            {
                return Problem("Entity set 'EcommerceContext.TempCart'  is null.");
            }

            var inventory = _context.Inventory.Find(tempCart.InventoryId);
            if(inventory == null)
            {
                return Problem("Item not found");
            }
            int stock = inventory.StockRemaining;
            if (stock >= tempCart.Quantity)
            {
                var cart = _mapper.Map<TempCartDto, TempCart>(tempCart);
                _context.TempCart.Add(cart);
                await _context.SaveChangesAsync();


            }
            else
            {
                return Problem("Error");
            }
            /*var tempCart = _mapper.Map<TempCartDto, TempCart>(tempCartDto);
            _context.TempCart.Add(tempCart);
            await _context.SaveChangesAsync();*/

            return CreatedAtAction("GetTempCart", new { id = tempCart.Id }, tempCart);
        }



        // DELETE: api/Example/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTempCart(int id)
        {
            if (_context.TempCart == null)
            {
                return NotFound();
            }
            var tempCart = await _context.TempCart.FindAsync(id);
            if (tempCart == null)
            {
                return NotFound();
            }
            var inventory = await _context.Inventory.FindAsync(tempCart.InventoryId);
            inventory.StockRemaining = inventory.StockRemaining + tempCart.Quantity;

            _context.TempCart.Remove(tempCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TempCartExists(int id)
        {
            return (_context.TempCart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
