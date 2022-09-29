using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    [Route("api/Tezeract")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly SurfsUpContext _context;

        public RentalsController(SurfsUpContext context)
        {
            _context = context;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rentings>>> GetRentings()
        {
            return await _context.Rentings.ToListAsync();
        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rentings>> GetRentings(int id)
        {
            var rentings = await _context.Rentings.FindAsync(id);

            if (rentings == null)
            {
                return NotFound();
            }

            return rentings;
        }
        //GET: api/Rentals
        //Checking if board is done being rented.
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
            return CreatedAtAction("Rent", new { id = board.Id }, board);
        }

        // DELETE: api/Rentals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentings(int id)
        {
            var rentings = await _context.Rentings.FindAsync(id);
            if (rentings == null)
            {
                return NotFound();
            }

            _context.Rentings.Remove(rentings);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentingsExists(int id)
        {
            return _context.Rentings.Any(e => e.Id == id);
        }
    }
}
