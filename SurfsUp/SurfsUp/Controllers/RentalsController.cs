using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
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

        // PUT: api/Rentals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentings(int id, Rentings rentings)
        {
            if (id != rentings.Id)
            {
                return BadRequest();
            }

            _context.Entry(rentings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Rentals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rentings>> PostRentings(Rentings rentings)
        {
            _context.Rentings.Add(rentings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRentings", new { id = rentings.Id }, rentings);
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
