using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.IServices
{
    public interface IDonationService
    {
        Task Add(Donation donation, string userId);
        Task<IEnumerable<Donation>> GetAllDonations();
        Task<Donation> GetDonationById(int id);
        Task<decimal> GetTotalDonationsAmount();
    }
}
