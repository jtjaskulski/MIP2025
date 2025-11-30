using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BombPol.Data;
using BombPol.Data.Entities;
using BombPol.Data.Extensions;

namespace BombPol.Intranet.Controllers
{
    public class NavigationLinkController : Controller
    {
        private readonly BombPolContext _context;

        public NavigationLinkController(BombPolContext context)
        {
            _context = context;
        }

        // GET: NavigationLink
        public async Task<IActionResult> Index()
        {
            return View(await _context.NavigationLinks.FilterOutDeleted().ToListAsync());
        }

        // GET: NavigationLink/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationLink = await _context.NavigationLinks
                .FilterOutDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationLink == null)
            {
                return NotFound();
            }

            return View(navigationLink);
        }

        // GET: NavigationLink/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NavigationLink/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LinkTitle,ControllerName,ControllerAction,Url,Id,DeletedAt,ModifiedAt,CreatedAt")] NavigationLink navigationLink)
        {
            if (ModelState.IsValid)
            {
                navigationLink.Id = Guid.NewGuid();
                _context.Add(navigationLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(navigationLink);
        }

        // GET: NavigationLink/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationLink = await _context.NavigationLinks
                .FilterOutDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationLink == null)
            {
                return NotFound();
            }
            return View(navigationLink);
        }

        // POST: NavigationLink/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LinkTitle,ControllerName,ControllerAction,Url,Id,DeletedAt,ModifiedAt,CreatedAt")] NavigationLink navigationLink)
        {
            if (id != navigationLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navigationLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavigationLinkExists(navigationLink.Id))
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
            return View(navigationLink);
        }

        // GET: NavigationLink/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationLink = await _context.NavigationLinks
                .FilterOutDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationLink == null)
            {
                return NotFound();
            }

            return View(navigationLink);
        }

        // POST: NavigationLink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var navigationLink = await _context.NavigationLinks
                .FilterOutDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationLink != null)
            {
                _context.NavigationLinks.Remove(navigationLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavigationLinkExists(Guid id)
        {
            return _context.NavigationLinks
                .FilterOutDeleted().Any(e => e.Id == id);
        }
    }
}
