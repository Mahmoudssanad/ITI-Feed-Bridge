using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Services
{
    public class NotificationService: INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetNewNotificationsCountAsync()
        {
            return await _context.Notifications.CountAsync(n => !n.IsRead);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
