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
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCart(int id)
        {
            if (_context.Cart.Where(c => c.OrderId == id) == null)
            {
                return NotFound();
            }
            var cart = _context.Cart.Where(c => c.OrderId == id).Include(c => c.Order).Include(c => c.Inventory).Include(c => c.User).ToList();

            if (cart == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<Cart>, List<CartDto>>(cart);
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

        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
