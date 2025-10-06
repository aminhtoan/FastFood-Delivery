using FastFood.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.BLL.Account
{
    public class AccountBLL
    {
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly UserManager<AppUserModel> _userManager;

        public AccountBLL(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message, string RedirectUrl)> LoginAsync(
            string username, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return (true, "Đăng nhập thành công!", "/Admin/Product");

                if (await _userManager.IsInRoleAsync(user, "Restaurant"))
                    return (true, "Đăng nhập thành công!", "/Restaurant/Dashboard");

                return (true, "Đăng nhập thành công!", "/Home/Index");
            }
            else if (result.IsLockedOut)
            {
                return (false, "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.", "");
            }
            else
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không đúng.", "");
            }
        }
        public async Task<(bool Success, string Message)> RegisterAsync(AppUserModel user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);

                return (true, "Đăng ký thành công");
            }

            string errorMsg = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, errorMsg);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
