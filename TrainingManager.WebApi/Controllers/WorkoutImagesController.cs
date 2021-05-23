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
    public class WorkoutImagesController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WorkoutImagesController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WorkoutImages
        [HttpGet]
        public IEnumerable<WorkoutImage> GetWorkoutImages()
        {
            return _context.WorkoutImages;
        }

        // GET: api/WorkoutImages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workoutImage = await _context.WorkoutImages.FindAsync(id);

            if (workoutImage == null)
            {
                return NotFound();
            }

            return Ok(workoutImage);
        }

        // PUT: api/WorkoutImages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutImage([FromRoute] int id, [FromBody] WorkoutImage workoutImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workoutImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(workoutImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutImageExists(id))
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

        // POST: api/WorkoutImages
        [HttpPost]
        public async Task<IActionResult> PostWorkoutImage([FromBody] WorkoutImage workoutImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WorkoutImages.Add(workoutImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkoutImage", new { id = workoutImage.Id }, workoutImage);
        }

        // DELETE: api/WorkoutImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workoutImage = await _context.WorkoutImages.FindAsync(id);
            if (workoutImage == null)
            {
                return NotFound();
            }

            _context.WorkoutImages.Remove(workoutImage);
            await _context.SaveChangesAsync();

            return Ok(workoutImage);
        }

        private bool WorkoutImageExists(int id)
        {
            return _context.WorkoutImages.Any(e => e.Id == id);
        }
    }
}