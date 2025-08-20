using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Tu.Models;
using Shopping_Tu.Repository;
using Shopping_Tu.Models.ViewModels;


namespace Shopping_Tutorial.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _signInManager = signInManager;
            _userManage = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("", " Lỗi tên đăng nhập hoặc mật khẩu");
                }
            }
            return View(loginVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newuser = new AppUserModel { UserName = user.Username, Email = user.Email, RoleId = "2" };
                IdentityResult result = await _userManage.CreateAsync(newuser, user.Password);
                if (result.Succeeded)
                {
                    var createUser = await _userManage.FindByEmailAsync(newuser.Email);
                    var userId = createUser.Id;
                    var role = _roleManager.FindByIdAsync(newuser.RoleId);
                    var addToRoleResult = await _userManage.AddToRoleAsync(createUser, role.Result.Name);
                    TempData["success"] = "Tạo user thành công";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        public async Task<IActionResult> History()
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _dataContext.Orders
                .Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
        }

        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders
                    .Where(o => o.OrderCode == ordercode).FirstAsync();
                order.Status = 2;
                _dataContext.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("History", "Account");
        }
    }

}
//mk : k123
