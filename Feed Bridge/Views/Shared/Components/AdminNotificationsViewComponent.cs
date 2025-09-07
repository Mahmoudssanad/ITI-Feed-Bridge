using Feed_Bridge.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Views.Shared.Component
{
    public class AdminNotificationsViewComponent :ViewComponent
    {
        private readonly INotificationService _notificationService;

        public AdminNotificationsViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            var newCount = await _notificationService.GetNewNotificationsCountAsync();

            ViewData["NewNotificationsCount"] = newCount;
            return View(notifications);
        }
    }
}
