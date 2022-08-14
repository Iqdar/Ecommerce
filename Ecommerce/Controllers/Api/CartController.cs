using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Areas.Identity.Data;
using Ecommerce.Models;
using Ecommerce.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EcommerceContext _context;
        private readonly SignInManager<EcommerceUser> _signInManager;
        private readonly UserManager<EcommerceUser> _userManager;

        public CartController(UserManager<EcommerceUser> userManager,
            SignInManager<EcommerceUser> signInManager, EcommerceContext context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Example
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCarts()
        {
            var cart = _context.Cart.ToList();
            if (cart == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<Cart>, List<CartDto>>(cart);
        }

        // GET: api/Example/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCart(int id)
        {
            if (_context.Cart == null)
            {
                return NotFound();
            }
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return _mapper.Map<Cart, CartDto>(cart);
        }

        // PUT: api/Example/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, CartDto cartDto)
        {
            if (id != cartDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var cart = _mapper.Map<CartDto, Cart>(cartDto);
            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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
        public async Task<ActionResult<CartDto>> PostCart(CartDto cartDto)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'EcommerceContext.Cart'  is null.");
            }
            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var cart = _mapper.Map<CartDto, Cart>(cartDto);
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cartDto.Id }, cartDto);
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

            _context.TempCart.Remove(tempCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
