using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DallalCore.Data;
using DallalCore.Models;

namespace DallalCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly DallalCoreContext _context;

        public RatesController(DallalCoreContext context)
        {
            _context = context;
        }

        // GET: api/Rates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rate>>> GetRate()
        {
            return await _context.Rate.Include(x=>x.user).Include(x=>x.product).ToListAsync();
        }

        // GET: api/Rates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rate>> GetRate(int id)
        {
            var rate = await _context.Rate.FindAsync(id);

            if (rate == null)
            {
                return NotFound();
            }

            return rate;
        }

        // PUT: api/Rates/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRate(int id, Rate rate)
        {
            if (id != rate.rateId)
            {
                return BadRequest();
            }

            _context.Entry(rate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RateExists(id))
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

        // POST: api/Rates
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Rate>> PostRate(Rate rate)
        {
            _context.Rate.Add(rate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRate", new { id = rate.rateId }, rate);
        }

        // DELETE: api/Rates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rate>> DeleteRate(int id)
        {
            var rate = await _context.Rate.FindAsync(id);
            if (rate == null)
            {
                return NotFound();
            }

            _context.Rate.Remove(rate);
            await _context.SaveChangesAsync();

            return rate;
        }

        private bool RateExists(int id)
        {
            return _context.Rate.Any(e => e.rateId == id);
        }
    }
}
