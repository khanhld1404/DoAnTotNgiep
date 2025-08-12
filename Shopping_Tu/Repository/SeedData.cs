
using Microsoft.EntityFrameworkCore;
using Shopping_Tu.Models;
using Shopping_Tu.Repository;

namespace Shopping_Tu.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel Do_bo_be_gai = new CategoryModel { Name = "Đồ bộ bé gái", Slug = "do bo be gai", Description = "Quan ao ca bo cho be nam", Status = 1 };
                CategoryModel Do_bo_be_trai = new CategoryModel { Name = "Đồ bộ bé trai", Slug = "do bo be trai", Description = "Quan ao ca bo cho be nu", Status = 1 };

                BrandModel Mama = new BrandModel { Name = "Mama", Slug = "mama", Description = "Hang quan ao tre em nu", Status = 1 };
                BrandModel Papa = new BrandModel { Name = "Papa", Slug = "papa", Description = "Hang quan ao tre em nam", Status = 1 };

                _context.Products.AddRange(

                    new ProductModel { Name = "Ao ngan tay", Slug = "ao ngan tay", Description = "Papa is best", Image = "1.jpg", Category = Do_bo_be_gai, Brand = Papa, Price = 122 },
                    new ProductModel { Name = "Vay hong xinh xan", Slug = "Vay hong xinh xan", Description = "Mama is best", Image = "2.jpg", Category = Do_bo_be_trai, Brand = Mama, Price = 122 }
                );
                _context.SaveChanges();
            }
        }
    }
}
