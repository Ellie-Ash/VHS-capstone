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
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikesController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            ViewBag.UserId = user.Id;
            var applicationDbContext = _context.Like
                .Include(l => l.Tape)
                .Include(l => l.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like
                .Include(l => l.Tape)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public async Task<IActionResult> CreateAsync(string Id)
        {
            var user = await GetCurrentUserAsync();
            var existingLike = await _context.Like
                .FirstOrDefaultAsync(l => l.LikeId.ToString() == Id && l.UserId == user.Id);
            if (existingLike == null)
            {
                var newLike = new Like
                {
                    //LikeId = Id,
                    UserId = user.Id
                };
                _context.Add(newLike);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Likes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikeId,UserId,TapeId")] Like like)
        {

            var user = await GetCurrentUserAsync();
            ModelState.Remove("LikeId");
            ModelState.Remove("UserId");
            ModelState.Remove("TapeId");

            if (ModelState.IsValid)
            {
                like.UserId = user.Id;
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TapeId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", like.TapeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", like.TapeId);
            return View(like);
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["TapeId"] = new SelectList(_context.Tape, "TapeId", "Description", like.TapeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", like.UserId);
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikeId,UserId,TapeId")] Like like)
        {
            if (id != like.LikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.LikeId))
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
            ViewData["TapeId"] = new SelectList(_context.Tape, "TapeId", "Description", like.TapeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", like.UserId);
            return View(like);
        }

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like
                .Include(l => l.Tape)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var like = await _context.Like.FindAsync(id);
            _context.Like.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
            return _context.Like.Any(e => e.LikeId == id);
        }
    }
}
