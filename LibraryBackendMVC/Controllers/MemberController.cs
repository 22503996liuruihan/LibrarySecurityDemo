using LibraryBackendMVC.Data;
using LibraryBackendMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Controllers
{
    /// <summary>
    /// 会员控制器，实现Member会员信息的CRUD增删改查
    /// </summary>
    public class MemberController : Controller
    {
        private readonly LibraryDbContext _db;

        public MemberController(LibraryDbContext db)
        {
            _db = db;
        }

        private bool IsLogin()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser"));
        }

        /// <summary>
        /// 会员列表页面
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var memberList = await _db.Members.ToListAsync();
            return View(memberList);
        }

        /// <summary>
        /// 新增会员页面
        /// </summary>
        public IActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            return View();
        }

        /// <summary>
        /// 提交新增会员
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _db.Members.Add(member);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        /// <summary>
        /// 编辑会员页面
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var member = await _db.Members.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        /// <summary>
        /// 提交编辑修改会员
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id != member.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(member);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        /// <summary>
        /// 删除确认页面
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (id == null) return NotFound();
            var member = await _db.Members.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        /// <summary>
        /// 执行删除会员
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            var member = await _db.Members.FindAsync(id);
            if (member != null)
            {
                _db.Members.Remove(member);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 判断会员是否存在
        /// </summary>
        private bool MemberExists(int id)
        {
            return _db.Members.Any(e => e.Id == id);
        }
    }
}