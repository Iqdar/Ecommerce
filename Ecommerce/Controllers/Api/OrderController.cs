using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Areas.Identity.Data;
using Ecommerce.Models;
using Ecommerce.Dtos;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Models.ViewModels;

namespace Ecommerce.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EcommerceContext _context;
        private readonly SignInManager<EcommerceUser> _signInManager;
        private readonly UserManager<EcommerceUser> _userManager;

        public OrderController(UserManager<EcommerceUser> userManager,
            SignInManager<EcommerceUser> signInManager, EcommerceContext context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Example
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var order = _context.Orders.Include(c => c.User).ToList();
            if (order == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<Order>, List<OrderDto>>(order);
        }

        // GET: api/Example/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(string userId)
        {
            if (_context.Orders.Where(c => c.UserId == userId) == null)
            {
                return NotFound();
            }
            var order = _context.Orders.Where(c => c.UserId == userId).ToList();

            if (order == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<Order>, List<OrderDto>>(order);
        }

        // PUT: api/Example/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrder(OrderDto orderDto)
        {

            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var order = _mapper.Map<OrderDto, Order>(orderDto);
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderDto.Id))
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
        [HttpPost("{userId}")]
        public async Task<ActionResult> PostOrder(string userId)
        {
            OrderDto orderDto;
            var tempCart = _context.TempCart.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
            if (tempCart != null)
            {
                int orderTotal = 0;
                for (int i = 0; i < tempCart.Count; i++)
                {
                    orderTotal = orderTotal + tempCart[i].Cost;
                }

                orderDto = new OrderDto();
                orderDto.UserId = userId;
                orderDto.Status = "Pending";
                orderDto.Bill = orderTotal;
                orderDto.PaymentStatus = false;


                var order = _mapper.Map<OrderDto, Order>(orderDto);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                int orderId = _context.Orders.Where(c => c.UserId == _userManager.GetUserId(User)).OrderByDescending(c => c.Id).FirstOrDefault().Id;

                CartDto cartDto;

                for (int i = 0; i < tempCart.Count; i++)
                {

                    cartDto = new CartDto();
                    cartDto.OrderId = orderId;
                    cartDto.UserId = userId;
                    cartDto.Quantity = tempCart[i].Quantity;
                    cartDto.Cost = tempCart[i].Cost;
                    cartDto.InventoryId = tempCart[i].InventoryId;

                    _context.Cart.Add(_mapper.Map<CartDto, Cart>(cartDto));
                    _context.TempCart.Remove(tempCart[i]);
                    await _context.SaveChangesAsync();
                }
            }

            /*if (_context.Orders == null)
            {
                return Problem("Entity set 'EcommerceContext.Order'  is null.");
            }
            if (!ModelState.IsValid)
            {
                return Problem("Model state Error.");
            }
            var order = _mapper.Map<OrderDto, Order>(orderDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();*/

            return NoContent();
        }


        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
