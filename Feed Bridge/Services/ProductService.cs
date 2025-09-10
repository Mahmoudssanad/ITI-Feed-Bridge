using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Products.Where(x => x.ExpirDate > today).ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Donation) // هات مع المنتج التبرع المرتبط
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }


        //public async Task<Product?> GetByIdAsync(int id)
        //{
        //    return await _context.Products.FindAsync(id);
        //}

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
