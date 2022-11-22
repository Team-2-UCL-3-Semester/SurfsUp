using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SurfsUpAPI.Data;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
	public class BoardController : Controller
	{
        private readonly SurfsUpContext _context;

        public BoardController(SurfsUpContext context)
        {
            _context = context;
        }


        // GET: BoardController
        public ActionResult Index()
		{
			return View();
		}

		// GET: BoardController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: BoardController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: BoardController/Create
		[HttpGet("Create")]
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
            }
            return Ok();
        }

        // GET: BoardController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            return View();
        }


        private bool BoardExists(Guid id)
        {
            return (_context.Board?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        // Virker ikke lige nu, kommer stadig ned til code 200 alligevel.
        // Skal gennemgåes
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,imgPath")] Board board)
        {
            if (id != board.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok();
        }

        // GET: BoardController/Delete/5
        public ActionResult Delete(int id)
		{
			return View();
		}

        //Delete works via API 22-11-2022
        // POST: BoardController/Delete/5
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board' is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
