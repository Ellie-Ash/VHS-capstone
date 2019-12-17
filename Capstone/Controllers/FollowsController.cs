using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone.Data;
using Capstone.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Controllers
{
    public class FollowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Follows
        public async Task<IActionResult> Index(string searchString)
        {
            var users = from f in _context.Follow
                           .Include(f => f.Follower)
                           .Include(f => f.User)
                           select f;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(f => f.User.UserName.Contains(searchString));
            }

            var user = await GetCurrentUserAsync();
            ViewBag.UserId = user.Id;
            return View(await users.ToListAsync());
        }

        // GET: Follows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow
                .Include(f => f.Follower)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FollowId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // GET: Follows/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await GetCurrentUserAsync();
            ViewData["UserId"] = user.Id;
            ViewData["FollowerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");

            return View();
        }

        // POST: Follows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowId,UserId,FollowerId")] Follow follow)
        {
 //Issues with this if valid && user is loggedin user
            var user = await GetCurrentUserAsync();
            ModelState.Remove("FollowId");
            ModelState.Remove("UserId");
            ModelState.Remove("FolllowerId");

            if (ModelState.IsValid)
            {
                follow.UserId = user.Id;
                _context.Add(follow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["FollowerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.FollowerId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.UserId);
            return View(follow);
        }

        // GET: Follows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow.FindAsync(id);
            if (follow == null)
            {
                return NotFound();
            }
            ViewData["FollowerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.FollowerId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.UserId);
            return View(follow);
        }

        // POST: Follows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FollowId,UserId,FollowerId")] Follow follow)
        {
            if (id != follow.FollowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(follow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowExists(follow.FollowId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FollowerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.FollowerId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", follow.UserId);
            return View(follow);
        }

        // GET: Follows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow
                .Include(f => f.Follower)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FollowId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var follow = await _context.Follow.FindAsync(id);
            _context.Follow.Remove(follow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowExists(int id)
        {
            return _context.Follow.Any(e => e.FollowId == id);
        }
    }
}
