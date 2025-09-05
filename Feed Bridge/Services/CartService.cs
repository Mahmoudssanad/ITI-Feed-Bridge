using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        //public async Task AddToCart(string userId, int productId, int quantity)
        //{
        //    var cartItem = await _context.Carts
        //        .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        //    if (cartItem != null)
        //    {
        //        cartItem.Quantity += quantity;
        //    }
        //    else
        //    {
        //        cartItem = new Cart
        //        {
        //            UserId = userId,
        //            ProductId = productId,
        //            Quantity = quantity
        //        };
        //        await _context.Carts.AddAsync(cartItem);
        //    }

        //    await _context.SaveChangesAsync();
        //}

        //public async Task<IEnumerable<Cart>> GetUserCart(string userId)
        //{
        //    return await _context.Carts
        //        .Include(c => c.Product)
        //        .Where(c => c.UserId == userId)
        //        .ToListAsync();
        //}

        //public async Task RemoveFromCart(int cartId)
        //{
        //    var cartItem = await _context.Carts.FindAsync(cartId);
        //    if (cartItem != null)
        //    {
        //        _context.Carts.Remove(cartItem);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task ClearCart(string userId)
        //{
        //    var cartItems = _context.Carts.Where(c => c.UserId == userId);
        //    _context.Carts.RemoveRange(cartItems);
        //    await _context.SaveChangesAsync();
        //}
    }
}
