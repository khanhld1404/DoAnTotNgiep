using System.ComponentModel.DataAnnotations;

namespace Shopping_Tu.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yeu cau nhap Ten Thuong hieu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yeu cau nhap Mo ta Thuong hieu")]
        public string Description { get; set; }


        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
