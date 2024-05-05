using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions;
using TrainingManager.WebApi.Controllers.Functions.Interfaces;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightActivitiesController : ControllerBase
    {
        private readonly TrainingManagerContext _context;
        private readonly IStatFunctions _statFunctions;
        private readonly IPersonalRecordHelperFunctions _personalRecordFunctions;

        public WeightActivitiesController(TrainingManagerContext context, IPersonalRecordHelperFunctions personalRecordFunctions, IStatFunctions statFunctions)
        {
            _context = context;
            _statFunctions = statFunctions;
            _personalRecordFunctions = personalRecordFunctions;
        }

        // GET: api/WeightActivities
        [HttpGet]
        public async Task<IActionResult> GetWeightActivities()
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            return Ok(_context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Select(x => new WeightActivityDTO()
            {
                ActivityGuid = x.ActivityGuid,
                ActivityName = x.ActivityName,
                MainMuscleGroup = _statFunctions.TryGetMuscle(x),
                IsWatched = x.IsWatched
            }).OrderBy(x => x.ActivityName));
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

        [HttpGet("MaxWeightActivity/{id}")]
        public IActionResult GetMaxWeightActivity([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(
                    _personalRecordFunctions.FindMaxMovedWeightsOfActivity(
                        _context.WeightActivities.Single(x => x.OwnerUserName == user.UserName && x.ActivityGuid == id).Id
                        )
                    );
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("LastRounds/{id}/{take}")]
        public IActionResult GetLastRounds([FromRoute] Guid id, int take)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var lastRounds = _statFunctions.GetLastRoundsOfActivity(_context.WeightActivities.Single(x => x.ActivityGuid == id).Id, user, take);

                if (lastRounds == null || lastRounds.Count() == 0)
                    return NotFound();

                return Ok(lastRounds);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetWatchedMaxWeightActivities")]
        public IActionResult GetWatchedMaxWeightActivities()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                return Ok(_personalRecordFunctions.FindMaxMovedWeightsByActivites(user));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("SearchActivity/{keyWords}")]
        public IActionResult SearchInWeightActivities([FromRoute] string keyWords)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                var allAvailableExercises = _context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Select(x => new WeightActivityDTO()
                {
                    ActivityGuid = x.ActivityGuid,
                    ActivityName = x.ActivityName,
                    MainMuscleGroup = _statFunctions.TryGetMuscle(x),
                    IsWatched = x.IsWatched
                });

                if (!string.IsNullOrEmpty(keyWords))
                {
                    string[] searchStrings = keyWords.Trim().Split(' ');
                    var foundElements = searchStrings.SelectMany(str => allAvailableExercises.Where(x => x.ActivityName.ToUpper().Contains(str.ToUpper())));
                    List<WeightActivityDTO> uniqueFoundElements = new List<WeightActivityDTO>();

                    foreach (var item in foundElements)
                    {
                        if (!uniqueFoundElements.Any(x => x.ActivityGuid == item.ActivityGuid))
                            uniqueFoundElements.Add(item);
                    }

                    return Ok(uniqueFoundElements.OrderBy(x => x.ActivityName));
                }
                else
                    return Ok(allAvailableExercises.OrderBy(x => x.ActivityName));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutWeightActivity([FromBody] WeightActivityDTO weightActivity)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var originalActivity = _context.WeightActivities.Single(u => u.OwnerUserName == user.UserName && u.ActivityGuid == weightActivity.ActivityGuid);

                if (originalActivity.ActivityName != weightActivity.ActivityName)
                    originalActivity.ActivityName = weightActivity.ActivityName;

                if (originalActivity.IsWatched != weightActivity.IsWatched)
                    originalActivity.IsWatched = weightActivity.IsWatched;

                if (originalActivity.MainMuscleGroup != weightActivity.MainMuscleGroup)
                    originalActivity.MainMuscleGroup = weightActivity.MainMuscleGroup;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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