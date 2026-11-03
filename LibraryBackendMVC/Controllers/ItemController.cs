using LibraryBackendMVC.Data;
using LibraryBackendMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Controllers
{
    /// <summary>
    /// 图书Item控制器，实现图书CRUD增删改查
    /// </summary>
    public class ItemController : Controller
    {
        private readonly LibraryDbContext _db;

        public ItemController(LibraryDbContext db)
        {
            _db = db;
        }

        private bool IsLogin()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser"));
        }

        /// <summary>
        /// 图书列表
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var list = await _db.Items.ToListAsync();
            return View(list);
        }

        /// <summary>
        /// 新增图书页面
        /// </summary>
        public IActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            return View();
        }

        /// <summary>
        /// 提交新增图书
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        /// <summary>
        /// 编辑图书页面
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var item = await _db.Items.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        /// <summary>
        /// 提交编辑图书
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id != item.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(item);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        /// <summary>
        /// 删除确认页面
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var item = await _db.Items.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        /// <summary>
        /// 执行删除图书
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var item = await _db.Items.FindAsync(id);
            if (item != null)
            {
                _db.Items.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _db.Items.Any(e => e.Id == id);
        }
    }
}