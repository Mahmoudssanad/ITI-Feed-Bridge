using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Services
{
    public class DonationService : IDonationService
    {
        private readonly AppDbContext _context;
        public DonationService(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _context = appDbContext;
        }


        public async Task Add(Donation donation, string userId)
        {
            donation.UserId = userId;

            await _context.Donations.AddAsync(donation);
            await _context.SaveChangesAsync();
        }



        public async Task<IEnumerable<Donation>> GetAllDonations()
        {
            return await  _context.Donations.ToListAsync();
        }

        public async Task< Donation> GetDonationById(int id)
        {
            return await _context.Donations.FirstOrDefaultAsync(d=>d.Id == id);
        }

        public async Task<decimal> GetTotalDonationsAmount()
        {
            return await _context.Donations.SumAsync(d => d.Quantity);
        }
    }
}
