using Feed_Bridge.Models.Entities;
using Feed_Bridge.Services;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Feed_Bridge.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user, password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Admin", "Admin");

                    else if (await _userManager.IsInRoleAsync(user, "Delivery"))
                        return RedirectToAction("Delivery", "Delivery");

                    else
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var exsitingUser = await _userManager.FindByEmailAsync(model.Email);
            if (exsitingUser != null)
            {
                ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم بالفعل");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    BirthDate = model.BirthDate
                };

                // رفع الصورة
                if (model.ImgFile != null && model.ImgFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImgFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImgFile.CopyToAsync(fileStream);
                    }

                    // خزّن الرابط في الداتابيز
                    user.ImgUrl = "/uploads/" + uniqueFileName;
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Profile", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "من فضلك أدخل البريد الإلكتروني";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "هذا البريد غير مسجل لدينا";
                return View();
            }

            // توليد رمز إعادة التعيين
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // إنشاء لينك لإعادة تعيين كلمة المرور
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token, email = user.Email }, Request.Scheme);

            // إعدادات الإيميل
            var fromAddress = new MailAddress("s04495320@gmail.com", "FeedBridge Support");
            var toAddress = new MailAddress(user.Email);
            const string fromPassword = "ajdw nbrm pndi zjmy"; // الباسورد اللي جبته من App Password
            string subject = "إعادة تعيين كلمة المرور - FeedBridge";
            //string body = $"مرحبا {user.UserName},\n\n" +
            //              $"اضغط على الرابط التالي لإعادة تعيين كلمة المرور:\n{resetLink}\n\n" +
            //              $"إذا لم تطلب ذلك، تجاهل هذه الرسالة.";
            string body = $@"
                            <!DOCTYPE html>
                            <html lang='ar'>
                            <head>
                              <meta charset='UTF-8'>
                              <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                              <style>
                                body {{
                                  font-family: 'Arial', sans-serif;
                                  background-color: #f5f5f5;
                                  padding: 20px;
                                  direction: rtl;
                                  text-align: center;
                                  margin: 0;
                                }}
                                .container {{
                                  max-width: 600px;
                                  margin: 0 auto;
                                  background-color: #ffffff;
                                  border-radius: 12px;
                                  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                                  overflow: hidden;
                                  text-align: center;
                                }}
                                .card-header {{
                                  background-color: #1cba7f;
                                  color: #fff;
                                  padding: 30px;
                                  border-top-left-radius: 12px;
                                  border-top-right-radius: 12px;
                                }}
                                .card-header h2 {{
                                  margin: 0;
                                  font-size: 24px;
                                  text-align: center;
                                }}
                                .card-body {{
                                  padding: 30px;
                                  color: #333;
                                  line-height: 1.8;
                                  text-align: center;
                                }}
                                .card-body p {{
                                  margin: 0 0 15px 0;
                                  text-align: center;
                                }}
                                .btn-wrapper {{
                                  padding: 0 30px 30px;
                                  text-align: center;
                                }}
                                .btn {{
                                  display: inline-block;
                                  padding: 12px 24px;
                                  font-size: 16px;
                                  color: #fff !important;
                                  background-color: #1cba7f;
                                  border-radius: 8px;
                                  text-decoration: none;
                                  transition: background-color 0.3s ease;
                                }}
                                .btn:hover {{
                                  background-color: #169363;
                                }}
                                .disclaimer {{
                                  margin-top: 20px;
                                  font-size: 12px;
                                  color: #777;
                                  text-align: center;
                                }}
                              </style>
                            </head>
                            <body>
                              <div class='container'>
                                <div class='card-header'>
                                  <h2>إعادة تعيين كلمة المرور</h2>
                                </div>
                                <div class='card-body'>
                                  <p>مرحباً <b>{user.UserName}</b>،</p>
                                  <p>اضغط على الزر أدناه لإعادة تعيين كلمة المرور الخاصة بك:</p>
                                  <div class='btn-wrapper'>
                                    <a href='{resetLink}' class='btn'>إعادة التعيين الآن</a>
                                  </div>
                                  <p class='disclaimer'>إذا لم تطلب ذلك، تجاهل هذه الرسالة.</p>
                                </div>
                              </div>
                            </body>
                            </html>";




            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }

            ViewBag.Message = "تم إرسال رابط إعادة تعيين كلمة المرور إلى بريدك الإلكتروني.";

            return View();
        }



        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ViewBag.Error = "الرابط غير صالح";
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "المستخدم غير موجود";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                ViewBag.Success = "تم تغيير كلمة المرور بنجاح";
                return RedirectToAction("Login");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View(model);
        }

    }
}
