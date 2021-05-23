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
    public class WeightWorkoutsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WeightWorkoutsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WeightWorkouts
        [HttpGet]
        public IEnumerable<WeightWorkout> GetWeightWorkouts()
        {
            return _context.WeightWorkouts;
        }

        // GET: api/WeightWorkouts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightWorkout([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightWorkout = await _context.WeightWorkouts.FindAsync(id);

            if (weightWorkout == null)
            {
                return NotFound();
            }

            return Ok(weightWorkout);
        }

        // PUT: api/WeightWorkouts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightWorkout([FromRoute] int id, [FromBody] WeightWorkout weightWorkout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightWorkout.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightWorkout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightWorkoutExists(id))
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

        // POST: api/WeightWorkouts
        [HttpPost]
        public async Task<IActionResult> PostWeightWorkout([FromBody] WeightWorkout weightWorkout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightWorkouts.Add(weightWorkout);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightWorkout", new { id = weightWorkout.Id }, weightWorkout);
        }

        // DELETE: api/WeightWorkouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightWorkout([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightWorkout = await _context.WeightWorkouts.FindAsync(id);
            if (weightWorkout == null)
            {
                return NotFound();
            }

            _context.WeightWorkouts.Remove(weightWorkout);
            await _context.SaveChangesAsync();

            return Ok(weightWorkout);
        }

        private bool WeightWorkoutExists(int id)
        {
            return _context.WeightWorkouts.Any(e => e.Id == id);
        }
    }
}