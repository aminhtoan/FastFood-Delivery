using FastFood.BLL.Account;
using FastFood.DAL.Models;
using FastFood.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountBLL _accountBLL;

        public AccountController(AccountBLL accountBLL)
        {
            _accountBLL = accountBLL;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, message, redirectUrl) = await _accountBLL.LoginAsync(
                model.Username,
                model.Password,
                model.RememberMe
            );

            if (success)
            {
                TempData["success"] = message;
                return Redirect(redirectUrl);
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUserModel
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName=model.FullName,
                    PhoneNumber=model.PhoneNumber
                    
                };

                var (success, message) = await _accountBLL.RegisterAsync(user, model.Password);

                if (success)

                    return RedirectToAction("Login");

                ModelState.AddModelError("", message);
            }

            return View(model);
        }
        [HttpGet]
      
        public async Task<IActionResult> Logout()
        {
            await _accountBLL.LogoutAsync();

            TempData["success"] = "Đăng xuất thành công!";
            return RedirectToAction("Login", "Account");
        }
    }
}
