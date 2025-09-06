using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.Services
{
    public class Partnerservice: IParteerService
    {
        private readonly AppDbContext _context;
        public Partnerservice(AppDbContext context)
        {
            _context = context;
        }
        public async Task Create(Partener partener)
        {
            _context.Parteners.Add(partener);
            await _context.SaveChangesAsync();
        }
    }
}
