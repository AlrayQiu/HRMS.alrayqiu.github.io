using HRMS_Application.DataBase;
using HRMS_Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS_Application.Controllers
{
    public class ReversoController : Controller
    {
        private readonly AppDbContext _context;

        public ReversoController(AppDbContext context)
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
                return View(new List<ReversoModel>());
            }
            if (UserModel.CurrentUser == null)
                UserModel.CurrentUser = _context.CreateUserModel(loggedInUsername);;
            return View(UserModel.CurrentUser.GetReversoList(_context));
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
