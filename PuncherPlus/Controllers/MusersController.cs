using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PuncherPlus.Models;

namespace PuncherPlus.Controllers
{
    public class MusersController : Controller
    {
        private readonly PuncherplusContext _context;

        public MusersController(PuncherplusContext context)
        {
            _context = context;
        }

        // GET: Musers
        public async Task<IActionResult> Index()
        {
              return _context.Musers != null ? 
                          View(await _context.Musers.ToListAsync()) :
                          Problem("Entity set 'PuncherplusContext.Musers'  is null.");
        }

        // GET: Musers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Musers == null)
            {
                return NotFound();
            }

            var muser = await _context.Musers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (muser == null)
            {
                return NotFound();
            }

            return View(muser);
        }

        // GET: Musers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nick,GivenName,FamilyName,CreateAt,UpdatedAt,DeletedAt")] Muser muser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(muser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(muser);
        }

        // GET: Musers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Musers == null)
            {
                return NotFound();
            }

            var muser = await _context.Musers.FindAsync(id);
            if (muser == null)
            {
                return NotFound();
            }
            return View(muser);
        }

        // POST: Musers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nick,GivenName,FamilyName,CreateAt,UpdatedAt,DeletedAt")] Muser muser)
        {
            if (id != muser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(muser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MuserExists(muser.Id))
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
            return View(muser);
        }

        // GET: Musers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Musers == null)
            {
                return NotFound();
            }

            var muser = await _context.Musers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (muser == null)
            {
                return NotFound();
            }

            return View(muser);
        }

        // POST: Musers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Musers == null)
            {
                return Problem("Entity set 'PuncherplusContext.Musers'  is null.");
            }
            var muser = await _context.Musers.FindAsync(id);
            if (muser != null)
            {
                _context.Musers.Remove(muser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MuserExists(int id)
        {
          return (_context.Musers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
