using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Tu.Repository;
using Shopping_Tutorial.Models;


namespace Shopping_Tu.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode == ordercode).ToListAsync();
            var Order = _dataContext.Orders
                .Where(od => od.OrderCode == ordercode).First();
            ViewBag.Status = Order.Status;
            return View(DetailsOrder);
        }
        public async Task<IActionResult> Delete(string ordercode)
        {
            // Tìm kiếm đơn hàng bằng cách sử dụng FirstOrDefaultAsync
            OrderModel order = await _dataContext.Orders
                .FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                TempData["error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index");
            }

            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xoá đơn hàng thành công";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Mỗi lỗi gặp phải trong quá trình cập nhật");
            }
        }
    }
}
