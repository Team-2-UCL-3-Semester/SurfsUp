using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using static System.Net.WebRequestMethods;

namespace SurfsUp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly SurfsUpContext _context;

        public BoardsController(SurfsUpContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int pg = 1)
        {

            //List<Board> surfBoards = _context.Board.ToList();

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["LengthSortParm"] = sortOrder == "Length" ? "length_desc" : "Length";
            ViewData["WidthSortParm"] = sortOrder == "Width" ? "width_desc" : "Width";
            ViewData["ThicknessSortParm"] = sortOrder == "Thickness" ? "thickness_desc" : "Thickness";
            ViewData["VolumeSortParm"] = sortOrder == "Volume" ? "volume_desc" : "Volume";
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";

            ViewData["IsRented"] = String.IsNullOrEmpty(sortOrder) ? "Rent_desc" : "";
            ViewData["CurrentFiltered"] = searchString;



            //DateTime temp = DateTime.Now;
            //if (DateTime.Compare(temp.AddSeconds(20), DateTime.Now) >= 0)
            //{
            //    board.IsRented = false;
            //}

            //var boards = from m in _context.Board
            //             select m;

            var boards = _context.Board.Where(s => !s.IsRented);

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name.Contains(searchString)
                                       || s.Type.Contains(searchString)
                                       || s.Equipment.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    boards = boards.OrderBy(m => m.Name);
                    break;
                case "price_desc":
                    boards = boards.OrderBy(m => m.Price);
                    break;
                case "length_desc":
                    boards = boards.OrderBy(m => m.Length);
                    break;
                case "width_desc":
                    boards = boards.OrderBy(m => m.Width);
                    break;
                case "thickness_desc":
                    boards = boards.OrderBy(m => m.Thickness);
                    break;
                case "volume_desc":
                    boards = boards.OrderBy(m => m.Volume);
                    break;
                case "type_desc":
                    boards = boards.OrderBy(m => m.Type);
                    break;
                case "isRented_desc":
                    boards = boards.OrderBy(m => m.IsRented);
                    break;
            }
            return View(await boards.ToListAsync());

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = boards.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = boards.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            // return View(surfBoards);

            return View(data);



            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name!.Contains(searchString));
            }
            return View(await boards.ToListAsync());

        }

        //[HttpPost, ActionName("Rent")]
        public async Task<IActionResult> Rent(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            board.IsRented = true;


            await _context.SaveChangesAsync();
            return View(Rent);

        }

        //GET: Boards/Details/5
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

        private bool BoardExists(Guid id)
        {
          return (_context.Board?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}


