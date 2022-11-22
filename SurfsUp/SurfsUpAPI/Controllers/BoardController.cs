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
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: BoardController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BoardController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: BoardController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
