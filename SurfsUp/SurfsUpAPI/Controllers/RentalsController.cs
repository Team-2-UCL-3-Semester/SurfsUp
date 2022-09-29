using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SurfsUp.Data;
using Microsoft.EntityFrameworkCore;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly SurfsUpContext _context;

        public RentalsController(SurfsUpContext context)
        {
            _context = context;
        }
        // POST: api/Rentals
        [HttpPost("rent")]
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
            return Ok();
        }

        //GET: api/Rentals
        //Checking if board is done being rented.
        [HttpGet("check")]
        public async Task<IActionResult> CheckRentals()
        {
            var boards = _context.Board.Where(s => !s.IsRented);
            var rentedBoards = _context.Board.Where(s => s.IsRented);
            foreach (var board in rentedBoards)
            {
                if (DateTime.Now >= board.RentedDate.Value.AddMinutes(2))
                {
                    board.IsRented = false;
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
