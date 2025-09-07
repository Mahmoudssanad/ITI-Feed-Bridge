using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.IServices
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<int> GetNewNotificationsCountAsync();
        Task MarkAsReadAsync(int id);
    }
}
