using HRMS_Application.DataBase;
using HRMS_Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? loggedInUsername = Request.Cookies["username"];
            // 如果没有 Cookie 或者值为空，可以设置默认值
            if (string.IsNullOrEmpty(loggedInUsername))
            {
                ViewBag.LoggedInUsername = "请登录";
                return View();
            }
            if (UserModel.CurrentUser == null)
                UserModel.CurrentUser = _context.CreateUserModel(loggedInUsername);
            ViewBag.LoggedInUsername = UserModel.CurrentUser.UserName;
            ViewBag.LoggedInMac = UserModel.CurrentUser.Mac;
            ViewBag.LoggedInTime = UserModel.CurrentUser.Time(_context);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
