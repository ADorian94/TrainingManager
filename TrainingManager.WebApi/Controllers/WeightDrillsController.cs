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
    public class WeightDrillsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WeightDrillsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WeightDrills
        [HttpGet]
        public IEnumerable<WeightDrill> GetWeightDrills()
        {
            return _context.WeightDrills;
        }

        // GET: api/WeightDrills/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightDrill([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightDrill = await _context.WeightDrills.FindAsync(id);

            if (weightDrill == null)
            {
                return NotFound();
            }

            return Ok(weightDrill);
        }

        // PUT: api/WeightDrills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightDrill([FromRoute] int id, [FromBody] WeightDrill weightDrill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightDrill.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightDrill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightDrillExists(id))
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

        // POST: api/WeightDrills
        [HttpPost]
        public async Task<IActionResult> PostWeightDrill([FromBody] WeightDrill weightDrill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightDrills.Add(weightDrill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightDrill", new { id = weightDrill.Id }, weightDrill);
        }

        // DELETE: api/WeightDrills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightDrill([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightDrill = await _context.WeightDrills.FindAsync(id);
            if (weightDrill == null)
            {
                return NotFound();
            }

            _context.WeightDrills.Remove(weightDrill);
            await _context.SaveChangesAsync();

            return Ok(weightDrill);
        }

        private bool WeightDrillExists(int id)
        {
            return _context.WeightDrills.Any(e => e.Id == id);
        }
    }
}