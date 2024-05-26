using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStoreAPIWebApp.Models;
using WebStoreAPIWebApp.Models.DTO;

namespace WebStoreAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly WebStoreAPIContext _context;

        public CartsController(WebStoreAPIContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, CartDTO cartDTO)
        {
            if (id != cartDTO.Id)
            {
                return BadRequest();
            }
            if (!CartExists(id))
            {
                return NotFound();
            }

            Cart cart = _context.Carts.Where(x => x.Id == id).First();


            cart.CustomerId = cartDTO.CustomerId;
            cart.DeliveryAddress = cartDTO.DeliveryAddress;

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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(CartDTO cartDTO)
        {
            Cart cart = new Cart
            {
                CustomerId = cartDTO.CustomerId,
                DeliveryAddress = cartDTO.DeliveryAddress,
                Price = 0,
                CartStatusId = _context.CartStatuses.Where(x => x.Name == "Не замовлений").First().Id,
                ProductCarts = [],
                Customer = null,
                CartStatus = null
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        private int CalculatePrice(Cart cart)
        {
            return _context.ProductCarts
                .Where(productCarts => productCarts.CartId == cart.Id)
                .Sum(productCarts => productCarts.Quantity * _context.Products
                    .Where(product => product.Id == productCarts.ProductId).First().Price);
        }
    }
}
