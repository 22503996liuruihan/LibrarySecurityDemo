using LibraryBackendMVC.Data;
using LibraryBackendMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Controllers
{
    /// <summary>
    /// 账号登录控制器
    /// </summary>
    public class AccountController : Controller
    {
        private readonly LibraryDbContext _db;
        public AccountController(LibraryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 提交登录校验
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            // 查询会员账号
            var admin = await _db.Members.FirstOrDefaultAsync(m => m.UserName == username && m.Password == password);
            if (admin != null)
            {
                // 登录成功，存入Session标记已登录
                HttpContext.Session.SetString("LoginUser", admin.UserName);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Msg = "账号或者密码错误！";
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoginUser");
            return RedirectToAction("Login");
        }
    }
}