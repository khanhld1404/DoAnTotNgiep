using System.ComponentModel.DataAnnotations.Schema;
using Shopping_Tu.Models;

namespace Shopping_Tutorial.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
