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
    public class WeightExercisesController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WeightExercisesController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WeightExercises
        [HttpGet]
        public IEnumerable<WeightExercise> GetWeightExercise()
        {
            return _context.WeightExercise;
        }

        // GET: api/WeightExercises/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightExercise([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightExercise = await _context.WeightExercise.FindAsync(id);

            if (weightExercise == null)
            {
                return NotFound();
            }

            return Ok(weightExercise);
        }

        // PUT: api/WeightExercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightExercise([FromRoute] int id, [FromBody] WeightExercise weightExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightExercise.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightExercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightExerciseExists(id))
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

        // POST: api/WeightExercises
        [HttpPost]
        public async Task<IActionResult> PostWeightExercise([FromBody] WeightExercise weightExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightExercise.Add(weightExercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightExercise", new { id = weightExercise.Id }, weightExercise);
        }

        // DELETE: api/WeightExercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightExercise([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightExercise = await _context.WeightExercise.FindAsync(id);
            if (weightExercise == null)
            {
                return NotFound();
            }

            _context.WeightExercise.Remove(weightExercise);
            await _context.SaveChangesAsync();

            return Ok(weightExercise);
        }

        private bool WeightExerciseExists(int id)
        {
            return _context.WeightExercise.Any(e => e.Id == id);
        }
    }
}