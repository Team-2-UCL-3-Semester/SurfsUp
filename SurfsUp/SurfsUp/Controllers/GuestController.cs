using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using SurfsUp.APIs;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SurfsUp.Controllers
{
    public class GuestController : Controller
    {
        private readonly SurfsUpContext _context;

        RentApiController rentApi = new();

        public GuestController(SurfsUpContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> Index(HttpClient client, string searchString)
        {
            

            //Checks if boards are still rented when index is opened
            //insert code here

            //Showing Boards, not rented
            client.BaseAddress = new Uri("https://localhost:7154");
            var boards = await client.GetFromJsonAsync<IEnumerable<Board>>("/Index");

           

          

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name!.Contains(searchString));
            }
            return View(boards);
        }

        // Rent Board
        public async Task<IActionResult> Rent(HttpClient httpClient, string? userId, Guid id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var board = await _context.Board
            .FirstOrDefaultAsync(m => m.Id == id);

            await rentApi.Rent(httpClient, userId, id);

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

