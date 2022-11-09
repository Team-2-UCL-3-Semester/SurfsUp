using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using static System.Net.WebRequestMethods;

namespace SurfsUp.Controllers
{
    public class AdminController : Controller
    {
        private readonly SurfsUpContext _context;

        public AdminController(SurfsUpContext context)
        {
            _context = context;
        }

        // GET: Boards

        public async Task<IActionResult> Index()
        {
            return _context.Board != null ?
                          View(await _context.Board.ToListAsync()) :
                          Problem("Entity set 'SurfsUpContext.Board'  is null.");
        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // GET: Boards/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,imgPath")] Board board)
        {
            if (board.imgPath == "" || board.imgPath == null)
            {
                board.imgPath = "https://thumbs.dreamstime.com/b/no-image-available-icon-photo-camera-flat-vector-illustration-132483141.jpg";
            }

            ModelState.Remove("RowVersion");

            if (ModelState.IsValid)
            {
                board.Id = Guid.NewGuid();
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }
            var board = await _context.Board.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,imgPath,RowVersion")] Board board)
        {
            if (id != board.Id)
            {
                return NotFound();
            }

            //Finder det Boardpost objekt fra databasen der matcher ID'et sendt fra viewet
            var boardToBeUpdated = await _context.Board.FirstOrDefaultAsync(x => x.Id == id);

            if (boardToBeUpdated == null)
            {
                Board deletedBoard = new Board();
                
                await TryUpdateModelAsync(deletedBoard);
                ModelState.AddModelError("", "Board ændringer kan ikke gemmes. En anden bruger har slettet boarded");

                return View(deletedBoard);
            }

            //Sætter original værdien for RowVersion,
            _context.Entry(boardToBeUpdated).Property("RowVersion").OriginalValue = board.RowVersion;

            if (await TryUpdateModelAsync<Board>(boardToBeUpdated, "",
                b => b.Name,
                b => b.Length,
                b => b.Thickness,
                b => b.Volume,
                b => b.Type,
                b => b.Equipment,
                b => b.Price,
                b => b.Width,
                b => b.imgPath,
                b => b.IsRented
                ))
            {
                try
                {
                    
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Board)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    //Hvis boardet er slettet i mellemtiden:
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Board ændringer kan ikke gemmes. En anden bruger har slettet boarded");
                    }
                    else
                    {
                        //Caster objektet til et Boardpost objekt
                        var databaseValue = (Board)databaseEntry.ToObject();

                        #region Fejlmeddelelser for hver textbox i view
                        if (clientValues.Name != databaseValue.Name)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Name}");
                        }
                        if (clientValues.Length != databaseValue.Length)
                        {
                            ModelState.AddModelError("Length", $"Nuværende værdi: {databaseValue.Length}");
                        }
                        if (clientValues.Width != databaseValue.Width)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Width}");
                        }
                        if (clientValues.Volume != databaseValue.Volume)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Volume}");
                        }
                        if (clientValues.Thickness != databaseValue.Thickness)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Thickness}");
                        }
                        if (clientValues.Type != databaseValue.Type)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Type}");
                        }
                        if (clientValues.imgPath != databaseValue.imgPath)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.imgPath}");
                        }
                        if (clientValues.Price != databaseValue.Price)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Price}");
                        }
                        if (clientValues.Equipment != databaseValue.Equipment)
                        {
                            ModelState.AddModelError("Name", $"Nuværende værdi: {databaseValue.Equipment}");
                        }
                        #endregion

                        ModelState.AddModelError("", "Kunne ikke gemme ændringerne." +
                            " En anden bruger har i mellemtiden lavet ændringer i dette board." +
                            " Ædringerne er vist i textboksene. Click på Save igen, for at gemme dine ændringer");
                        //Sætter RowVersion propertien for objektet til at være den nyere fra databasen
                        boardToBeUpdated.RowVersion = (byte[])databaseValue.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            return View(boardToBeUpdated);
         
        }

        // GET: Boards/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(Guid id)
        {
            return (_context.Board?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
