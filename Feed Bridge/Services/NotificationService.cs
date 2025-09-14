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

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                                              .Where(n => n.UserId == userId && !n.IsRead)
                                              .ToListAsync();

            if (notifications.Any())
            {
                foreach (var notif in notifications)
                {
                    notif.IsRead = true;
                }

                await _context.SaveChangesAsync();
            }
        }

        // إضافة إشعار جديد
      

        // جلب عدد الإشعارات غير المقروءة
        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                                 .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
