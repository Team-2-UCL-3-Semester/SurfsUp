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

        // GET: Boards

        public async Task<IActionResult> Index(string searchString)
            {
            var boards = from m in _context.Board
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name!.Contains(searchString));
            }
            return View(await boards.ToListAsync());

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

        private bool BoardExists(Guid id)
        {
          return (_context.Board?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        

    }
}


