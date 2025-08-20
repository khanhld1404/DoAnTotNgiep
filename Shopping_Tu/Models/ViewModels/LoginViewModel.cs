using System.ComponentModel.DataAnnotations;

namespace Shopping_Tu.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên người dùng")]
        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Nhập mật khẩu ")]
        public string Password { get; set; }

        public String ReturnUrl { get; set; }

    }
}
