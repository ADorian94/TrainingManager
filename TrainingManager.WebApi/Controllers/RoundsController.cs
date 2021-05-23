using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public RoundsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/Rounds
        [HttpGet]
        public IEnumerable<Round> GetWeightRounds()
        {
            return _context.WeightRounds;
        }

        // GET: api/Rounds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRound([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var round = await _context.WeightRounds.FindAsync(id);

            if (round == null)
            {
                return NotFound();
            }

            return Ok(round);
        }

        // PUT: api/Rounds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRound([FromRoute] int id, [FromBody] Round round)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != round.Id)
            {
                return BadRequest();
            }

            _context.Entry(round).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoundExists(id))
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

        // POST: api/Rounds
        [HttpPost]
        public async Task<IActionResult> PostRound([FromBody] Round round)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightRounds.Add(round);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRound", new { id = round.Id }, round);
        }

        // DELETE: api/Rounds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRound([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var round = await _context.WeightRounds.FindAsync(id);
            if (round == null)
            {
                return NotFound();
            }

            _context.WeightRounds.Remove(round);
            await _context.SaveChangesAsync();

            return Ok(round);
        }

        private bool RoundExists(int id)
        {
            return _context.WeightRounds.Any(e => e.Id == id);
        }
    }
}