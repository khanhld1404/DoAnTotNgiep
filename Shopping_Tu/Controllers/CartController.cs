using Microsoft.AspNetCore.Mvc;

namespace Shopping_Tu.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
