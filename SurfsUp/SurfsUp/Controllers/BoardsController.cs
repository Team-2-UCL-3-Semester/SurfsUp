using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
            //Sort Order
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["LengthSortParm"] = sortOrder == "Length" ? "length_desc" : "Length";
            ViewData["WidthSortParm"] = sortOrder == "Width" ? "width_desc" : "Width";
            ViewData["ThicknessSortParm"] = sortOrder == "Thickness" ? "thickness_desc" : "Thickness";
            ViewData["VolumeSortParm"] = sortOrder == "Volume" ? "volume_desc" : "Volume";
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";

            ViewData["IsRented"] = String.IsNullOrEmpty(sortOrder) ? "Rent_desc" : "";
            ViewData["CurrentFiltered"] = searchString;

            //Showing Boards, not rented
            var boards = _context.Board.Where(s => !s.IsRented);
            var rentedBoards = _context.Board.Where(s => s.IsRented);
            
            //Checking if board is done being rented.
            foreach (var board in rentedBoards)
            {
                if (DateTime.Now >= board.RentedDate.Value.AddSeconds(20))
                {
                    board.IsRented = false;
                }
            }
            await _context.SaveChangesAsync();

            //Filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name.Contains(searchString)
                                       || s.Type.Contains(searchString)
                                       || s.Equipment.Contains(searchString));
            }

            //Sorting
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

            //PageCounter
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
            
            return View(data);

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name!.Contains(searchString));
            }
            return View(await boards.ToListAsync());
        }

        //[HttpPost, ActionName("Rent")]
        public async Task<IActionResult> Rent(Guid? id, Guid Id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (board.IsRented)
            {
                return NotFound();
            }
            
            board.IsRented = true;
            board.RentedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            // Ide fra Jaco og denne video - https://www.youtube.com/watch?v=qRvIVSV4YuM
            // Vi tager nu userID fra den user der er logget ind ved claims.Value
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            _context.SaveRenting(DateTime.Now, DateTime.Now.AddMinutes(1), claims.Value, Id);
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


