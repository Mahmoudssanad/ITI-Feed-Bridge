using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.IServices
{
    public interface ICartService
    {
        public Task<(bool Success, string Message)> AddToCart(string userId, int productId, int quantity);
        Task<Cart> GetUserCart(string userId);
        //Task RemoveFromCart(int cartId);
        //Task ClearCart(string userId);
    }
}
