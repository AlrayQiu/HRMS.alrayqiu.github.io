using System;

using System.Collections.Generic;

using System.Linq;

using System.Security.Claims;
using System.Threading.Tasks;
using HRMS_Application.DataBase;
using HRMS_Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Server.Controllers
{
    public class AccountController(AppDbContext dBContext) : Controller
    {
        private readonly AppDbContext _context = dBContext;
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// post 登录请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Json(new { result = false, msg = "请输入用户名或密码!" });
            }
            else if (_context.VerifyUser(userName,password))
            {
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name,userName),new Claim("password",password)
                };
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });
                Response.Cookies.Append("username", userName, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
                UserModel.CurrentUser = _context.CreateUserModel(userName);
                return Redirect("/Home/Index");
            }
            return Json(new { result = false, msg = "用户名密码错误!" });
        }



        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
    }
}
