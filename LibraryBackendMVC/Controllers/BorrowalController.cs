using LibraryBackendMVC.Data;
using LibraryBackendMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Controllers
{
    /// <summary>
    /// 借阅记录控制器，实现Borrowal的CRUD
    /// </summary>
    public class BorrowalController : Controller
    {
        private readonly LibraryDbContext _db;

        public BorrowalController(LibraryDbContext db)
        {
            _db = db;
        }

        private bool IsLogin()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser"));
        }

        /// <summary>
        /// 借阅记录列表
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var list = await _db.Borrowals
                .Include(b => b.Member)
                .Include(b => b.Item)
                .ToListAsync();
            return View(list);
        }

        /// <summary>
        /// 新增页面
        /// </summary>
        public async Task<IActionResult> Create()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            ViewData["MemberId"] = await _db.Members.ToListAsync();
            ViewData["ItemId"] = await _db.Items.ToListAsync();
            return View();
        }

        /// <summary>
        /// 提交新增
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Borrowal borrowal)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _db.Borrowals.Add(borrowal);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = await _db.Members.ToListAsync();
            ViewData["ItemId"] = await _db.Items.ToListAsync();
            return View(borrowal);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var borrowal = await _db.Borrowals.FindAsync(id);
            if (borrowal == null) return NotFound();

            ViewData["MemberId"] = await _db.Members.ToListAsync();
            ViewData["ItemId"] = await _db.Items.ToListAsync();
            return View(borrowal);
        }

        /// <summary>
        /// 提交编辑修改
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Borrowal borrowal)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id != borrowal.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(borrowal);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowalExists(borrowal.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = await _db.Members.ToListAsync();
            ViewData["ItemId"] = await _db.Items.ToListAsync();
            return View(borrowal);
        }

        /// <summary>
        /// 删除确认页面
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var borrowal = await _db.Borrowals
                .Include(b => b.Member)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (borrowal == null) return NotFound();
            return View(borrowal);
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var borrowal = await _db.Borrowals.FindAsync(id);
            if (borrowal != null)
            {
                _db.Borrowals.Remove(borrowal);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowalExists(int id)
        {
            return _db.Borrowals.Any(e => e.Id == id);
        }
    }
}