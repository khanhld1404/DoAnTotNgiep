using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shopping_Tu.Models;
using Shopping_Tu.Models.ViewModels;
using Shopping_Tu.Repository;
using Shopping_Tutorial.Models;

namespace Shopping_Tu.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        public CheckoutController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy giỏ hàng
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
                                            ?? new List<CartItemModel>();
            if (cartItems.Count == 0)
            {
                TempData["error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            // Kiểm tra tồn kho
            foreach (var cart in cartItems)
            {
                var product = await _dataContext.Products.FindAsync(cart.ProductId);
                if (product == null)
                {
                    TempData["error"] = $"Sản phẩm {cart.ProductId} không tồn tại!";
                    return RedirectToAction("Index", "Cart");
                }
                if (cart.Quantity > product.Quantity)
                {
                    TempData["error"] = $"Sản phẩm {product.Name} không đủ số lượng!";
                    return RedirectToAction("Index", "Cart");
                }
            }

            // Tạo Order
            var orderCode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel
            {
                OrderCode = orderCode,
                UserName = userEmail,
                Status = 1,
                CreatedDate = DateTime.Now
            };
            _dataContext.Add(orderItem);

            // Tạo OrderDetails và trừ tồn kho
            foreach (var cart in cartItems)
            {
                var product = await _dataContext.Products.FindAsync(cart.ProductId);

                var orderDetails = new OrderDetails
                {
                    UserName = userEmail,
                    OrderCode = orderCode,
                    ProductId = cart.ProductId,
                    ProductName = cart.ProductName,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                _dataContext.Add(orderDetails);

                // Trừ tồn kho
                product.Quantity -= cart.Quantity;
                product.Quantity = Math.Max(product.Quantity, 0); // tránh âm
                // Không cần Update() vì EF đang track entity
            }

            await _dataContext.SaveChangesAsync(); // commit 1 lần

            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Đặt hàng thành công, số lượng sản phẩm đã được cập nhật!";
            return RedirectToAction("History", "Account");
        }
    }
}
