using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Data;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public IActionResult AllUsers()
        {
            
            return View();
        }
        public async Task<IActionResult> Index(string id)
        {
            var user = await GetCurrentUserAsync();
            ViewBag.UserId = user.Id;
            if (id == null)
            {
                return NotFound();
            }
            var profile = await _context.Users
                .Include(u => u.Tapes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }
        // GET: Tapes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tape = await _context.Tape
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TapeId == id);
            if (tape == null)
            {
                return NotFound();
            }

            return View(tape);
        }

        [Authorize]
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize]
        // POST: Tapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TapeId,UserId,Title,Description,Genre,Year,ImagePath,Link,isAvailable")] Tape tape)
        {
            var user = await GetCurrentUserAsync();
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            if (id != tape.TapeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    tape.UserId = user.Id;
                    _context.Update(tape);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TapeExists(tape.TapeId))
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

            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", tape.UserId);
            return View(tape);
        }




        [Authorize]
        // GET: Tapes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        [Authorize]
        // POST: Tapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TapeId,UserId,Title,Description,Genre,Year,ImagePath,Link,isAvailable")] Tape tape)
        {
            var user = await GetCurrentUserAsync();
            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                tape.UserId = user.Id;
                _context.Add(tape);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", tape.UserId);
            return View(tape);
        }
        private bool TapeExists(int id)
        {
            return _context.Tape.Any(e => e.TapeId == id);
        }
    }
}