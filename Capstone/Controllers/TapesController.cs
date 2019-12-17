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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Capstone.Models.ViewModels;

namespace Capstone.Controllers
{
    public class TapesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public TapesController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = ctx;
            hostingEnvironment = environment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Tapes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tape
                .Include(t => t.User);
                
            var user = await GetCurrentUserAsync();
            ViewBag.UserId = user.Id;
            return View(await applicationDbContext.ToListAsync());
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
        // GET: Tapes/Create
        public IActionResult Create()
        {
            var viewModel = new CreateTapeViewModel();
            return View(viewModel);
        }

        [Authorize]
        // POST: Tapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTapeViewModel viewModel)
        {
            
            var user = await GetCurrentUserAsync();
            ModelState.Remove("Tape.User");
            ModelState.Remove("Tape.UserId");
            ModelState.Remove("Tape.TapeId");
            

            var img = viewModel.MyImage;

            if (ModelState.IsValid)
            {
            if (img != null)
            {
            var uniqueFileName = GetUniqueFileName(viewModel.MyImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "userImg");
                var filePath = Path.Combine(uploads, uniqueFileName);
            var fileName = Path.GetFileName(viewModel.MyImage.FileName);
                    using (var myFile = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.MyImage.CopyTo(myFile);
                    }
                var contentType = viewModel.MyImage.ContentType;
                viewModel.Tape.UserId = user.Id;
                viewModel.Tape.ImagePath = uniqueFileName;
            }
                _context.Add(viewModel.Tape);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        [Authorize]
        // GET: Tapes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
    }

            var tape = await _context.Tape.FindAsync(id);
            if (tape == null)
            {
                return NotFound();
}

            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", tape.UserId);
            return View(tape);
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
        // GET: Tapes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Tapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var tape = await _context.Tape.FindAsync(id);
            var fileName = tape.ImagePath;
            var images = Directory.GetFiles("wwwroot/userImg");
            var fileToDelete = images.First(i => i.Contains(fileName));
            System.IO.File.Delete(fileToDelete);


            _context.Tape.Remove(tape);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TapeExists(int id)
        {
            return _context.Tape.Any(e => e.TapeId == id);
        }
    }
}
