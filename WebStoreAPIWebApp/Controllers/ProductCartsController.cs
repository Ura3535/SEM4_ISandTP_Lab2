using System;
using System.Collections.Generic;
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
    public class ProductCartsController : ControllerBase
    {
        private readonly WebStoreAPIContext _context;

        public ProductCartsController(WebStoreAPIContext context)
        {
            _context = context;
        }

        // GET: api/ProductCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCart>>> GetProductCarts()
        {
            return await _context.ProductCarts.ToListAsync();
        }

        // GET: api/ProductCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCart>> GetProductCart(int id)
        {
            var productCart = await _context.ProductCarts.FindAsync(id);

            if (productCart == null)
            {
                return NotFound();
            }

            return productCart;
        }

        // PUT: api/ProductCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCart(int id, ProductCartDTO productCartDTO)
        {
            if (id != productCartDTO.Id)
            {
                return BadRequest();
            }
            if (!ProductCartExists(id) || !CartExists(productCartDTO.CartId))
            {
                return NotFound();
            }

            ProductCart productCart = await _context.ProductCarts.FindAsync(productCartDTO.Id);

            productCart.ProductId = productCartDTO.ProductId;
            productCart.CartId = productCartDTO.CartId;
            productCart.Quantity = productCartDTO.Quantity;
            _context.Entry(productCart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Cart cart = await _context.Carts.FindAsync(productCartDTO.CartId);
            cart.Price = CalculatePrice(cart);

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCartExists(id))
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

        // POST: api/ProductCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCart>> PostProductCart(ProductCartDTO productCartDTO)
        {
            if (!CartExists(productCartDTO.CartId))
            {
                return NotFound();
            }

            ProductCart productCart = new ProductCart
            {
                ProductId = productCartDTO.ProductId,
                CartId = productCartDTO.CartId,
                Quantity = productCartDTO.Quantity,
            };

            _context.ProductCarts.Add(productCart);
            await _context.SaveChangesAsync();

            Cart cart = await _context.Carts.FindAsync(productCartDTO.CartId);
            cart.Price = CalculatePrice(cart);

            _context.Entry(cart).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCart", new { id = productCart.Id }, productCart);
        }

        // DELETE: api/ProductCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCart(int id)
        {
            var productCart = await _context.ProductCarts.FindAsync(id);
            if (productCart == null)
            {
                return NotFound();
            }

            _context.ProductCarts.Remove(productCart);
            await _context.SaveChangesAsync();

            Cart cart = await _context.Carts.FindAsync(productCart.CartId);
            cart.Price = CalculatePrice(cart);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCartExists(int id)
        {
            return _context.ProductCarts.Any(e => e.Id == id);
        }
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        private int CalculatePrice(Cart cart)
        {
            return _context.ProductCarts
            .Where(productCarts => productCarts.CartId == cart.Id)
            .Join(_context.Products,
                productCart => productCart.ProductId,
                product => product.Id,
                (productCart, product) => new { productCart.Quantity, product.Price })
            .Sum(pc => pc.Quantity * pc.Price);
        }
    }
}
