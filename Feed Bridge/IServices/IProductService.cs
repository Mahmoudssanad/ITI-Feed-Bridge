using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.IServices
{
    public interface IProductService
    {
            Task<IEnumerable<Product>> GetAllAsync(string category );
            Task<Product?> GetByIdAsync(int id);
            Task AddAsync(Product product);
            Task UpdateAsync(Product product);
            Task DeleteAsync(int id);
        
    }
}
