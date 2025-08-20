using System.ComponentModel.DataAnnotations;

namespace Shopping_Tu.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên người dùng")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Nhập email"), EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Nhập mật khẩu ")]
        public string Password { get; set; }

    }
}
