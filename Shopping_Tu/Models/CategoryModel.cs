using System.ComponentModel.DataAnnotations;

namespace Shopping_Tu.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yeu cau nhap Ten Danh muc")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yeu cau nhap Mo ta Danh muc")]
        public string Description { get; set; }

        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
