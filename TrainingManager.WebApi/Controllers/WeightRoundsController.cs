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
    public class WeightRoundsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WeightRoundsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WeightRounds
        [HttpGet]
        public IEnumerable<WeightRound> GetWeightRounds()
        {
            return _context.WeightRounds;
        }

        // GET: api/WeightRounds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightRound([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightRound = await _context.WeightRounds.FindAsync(id);

            if (weightRound == null)
            {
                return NotFound();
            }

            return Ok(weightRound);
        }

        // PUT: api/WeightRounds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightRound([FromRoute] int id, [FromBody] WeightRound weightRound)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightRound.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightRound).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightRoundExists(id))
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

        // POST: api/WeightRounds
        [HttpPost]
        public async Task<IActionResult> PostWeightRound([FromBody] WeightRound weightRound)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightRounds.Add(weightRound);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightRound", new { id = weightRound.Id }, weightRound);
        }

        // DELETE: api/WeightRounds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightRound([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightRound = await _context.WeightRounds.FindAsync(id);
            if (weightRound == null)
            {
                return NotFound();
            }

            _context.WeightRounds.Remove(weightRound);
            await _context.SaveChangesAsync();

            return Ok(weightRound);
        }

        private bool WeightRoundExists(int id)
        {
            return _context.WeightRounds.Any(e => e.Id == id);
        }
    }
}