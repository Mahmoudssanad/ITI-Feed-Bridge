using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Feed_Bridge.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public ReportController(IReportService reportService, UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _reportService = reportService;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allReports = await _reportService.GetAll();
            return View(allReports);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var donors = await _userManager.Users
        .Where(u => u.Supports.Any()) // اللي تبرعوا بس
        .ToListAsync();

            var vm = new ReportViewModel
            {
                Donors = donors
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var donor = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == model.SelectedUserId);
                if (donor == null)
                {
                    ModelState.AddModelError("", "المتبرع غير موجود");
                    return View(model);
                }

                var report = new Report
                {
                    title = model.Title,
                    content = model.Content,
                    Email = donor.Email, // ناخده أوتوماتيك
                    CreatedAt = DateTime.Now,
                    UserId = _userManager.GetUserId(User) // الأدمن اللي أنشأ التقرير
                };

                await _reportService.Create(report);

                // إرسال الإيميل للمتبرع
                await _emailService.SendEmailAsync(
                    donor.Email,
                    model.Title,
                    model.Content
                );

                TempData["SuccessMessage"] = "تم إرسال التقرير للمتبرع بنجاح";
                return RedirectToAction("Reports", "Admin");
            }
            ModelState.AddModelError("", "Error");
            // لو فيه مشكلة نرجع القائمة تاني
            model.Donors = await _userManager.Users
                .Where(u => u.Supports.Any())
                .ToListAsync();

            return View(model);
        }
    }
}
