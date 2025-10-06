using System.ComponentModel.DataAnnotations;

namespace FastFood.UI.ViewModels
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Hãy nhập đầy đủ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hãy nhập đầy đủ")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập đầy đủ")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
