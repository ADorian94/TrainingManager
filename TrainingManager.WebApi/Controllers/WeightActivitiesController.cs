using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightActivitiesController : ControllerBase
    {
        private readonly TrainingManagerContext _context;
        private readonly StatFunctions _statFunctions;

        public WeightActivitiesController(TrainingManagerContext context)
        {
            _context = context;
            _statFunctions = new StatFunctions(context);
        }

        // GET: api/WeightActivities
        [HttpGet]
        public async Task<IActionResult> GetWeightActivities()
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            return Ok(_context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Select(x => new WeightActivityDTO()
            {
                ActivityName = x.ActivityName,
                MainMuscleGroup = _statFunctions.TryGetMuscle(x)
            }));
        }

        // GET: api/WeightActivities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeightActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightActivity = await _context.WeightActivities.FindAsync(id);

            if (weightActivity == null)
            {
                return NotFound();
            }

            return Ok(weightActivity);
        }

        // PUT: api/WeightActivities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightActivity([FromRoute] int id, [FromBody] WeightActivity weightActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightActivityExists(id))
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

        // POST: api/WeightActivities
        [HttpPost]
        public async Task<IActionResult> PostWeightActivity([FromBody] WeightActivity weightActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightActivities.Add(weightActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightActivity", new { id = weightActivity.Id }, weightActivity);
        }

        // DELETE: api/WeightActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weightActivity = await _context.WeightActivities.FindAsync(id);
            if (weightActivity == null)
            {
                return NotFound();
            }

            _context.WeightActivities.Remove(weightActivity);
            await _context.SaveChangesAsync();

            return Ok(weightActivity);
        }

        private bool WeightActivityExists(int id)
        {
            return _context.WeightActivities.Any(e => e.Id == id);
        }
    }
}