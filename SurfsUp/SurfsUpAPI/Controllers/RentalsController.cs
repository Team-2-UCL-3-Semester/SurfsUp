using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SurfsUp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    
    [ApiController]
    [Route("/")]
    public class RentalsController : ControllerBase
    {
        private readonly SurfsUpContext _context;

        public RentalsController(SurfsUpContext context)
        {
            _context = context;
        }
        // POST: api/Rentals
        [HttpGet("rent")]
        public async Task<IActionResult> Rent(string userId, Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);

            if(board == null)
            {
                return Ok();
            }
            else
            {
                if (board.IsRented)
                {
                    return NotFound();
                }
            }

            // Ide fra Jaco og denne video - https://www.youtube.com/watch?v=qRvIVSV4YuM
            // Vi tager nu userID fra den user der er logget ind ved claims.Value

            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //userId = Convert.ToString(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier));

            if (userId == null || userId == "")
            {
                board.IsRented = true;
                board.RentedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                _context.SaveRenting(DateTime.Now, DateTime.Now.AddMinutes(1), "9e7aa49f-1b34-468e-b60d-dd2541c9694e", id);
            }
            else
            {
                board.IsRented = true;
                board.RentedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                _context.SaveRenting(DateTime.Now, DateTime.Now.AddMinutes(1), userId, id);
            }
            return Ok();
        }

        //GET: api/Rentals
        //Checking if board is done being rented.
        [HttpGet("check")]
        public async Task<IActionResult> CheckRentals()
        {
            var rentedBoards = _context.Board.Where(s => s.IsRented);
            foreach (var board in rentedBoards)
            {
                if (DateTime.Now >= board.RentedDate.Value.AddSeconds(10))
                {
                    board.IsRented = false;
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }




       [HttpGet("Index")]
        public  IActionResult Index()
        {
            var boards = _context.Board.Where(s => !s.IsRented);

            return Ok(boards);
        }
      
    }
}
