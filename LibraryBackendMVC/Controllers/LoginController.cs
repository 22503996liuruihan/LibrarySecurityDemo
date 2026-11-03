using LibraryBackendMVC.Data;
using LibraryBackendMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Controllers;

public class LoginController : Controller
{
    private readonly LibraryDbContext _db;
    public LoginController(LibraryDbContext db)
    {
        _db = db;
    }

    // 登录页面
    public IActionResult Index()
    {
        return View();
    }

    // 提交登录表单
    [HttpPost]
    public async Task<IActionResult> Index(string userName, string password)
    {
        // 查询用户
        var user = await _db.Members.FirstOrDefaultAsync(m => m.UserName == userName && m.Password == password);
        if (user == null)
        {
            ViewBag.Msg = "账号或密码错误！";
            return View();
        }

        // 登录成功，存入Session
        HttpContext.Session.SetString("UserName", user.UserName);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToAction("Index", "Item");
    }

    // 退出登录
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}