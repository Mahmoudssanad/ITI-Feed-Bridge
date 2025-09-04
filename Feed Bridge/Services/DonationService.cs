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
        public async Task DeleteDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateDonation(Donation donation)
        {
            var existingDonation = await _context.Donations.FindAsync(donation.Id);
            if (existingDonation != null)
            {
                existingDonation.Name = donation.Name;
                existingDonation.ImgURL = donation.ImgURL;
                existingDonation.ExpirDate = donation.ExpirDate;
                existingDonation.Quantity = donation.Quantity;
                existingDonation.Address = donation.Address;
                existingDonation.Phone = donation.Phone;
                existingDonation.Description = donation.Description;

                _context.Donations.Update(existingDonation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
