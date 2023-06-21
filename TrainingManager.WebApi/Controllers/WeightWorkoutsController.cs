using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace TrainingManager.WebApi.Controllers
{
#warning Refaktorálni
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeightWorkoutsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;
        private readonly StatFunctions _statFunctions;

        public WeightWorkoutsController(TrainingManagerContext context)
        {
            _context = context;
            _statFunctions = new StatFunctions(context);
        }

        // GET: api/WeightWorkouts
        [HttpGet]
        public IActionResult GetWeightWorkouts()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(GetUserWorkouts(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/WeightWorkouts/5
        [HttpGet("{id}")]
        public IActionResult GetWeightWorkout([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                return Ok(_context.WeightWorkouts.AsNoTracking().Where(x => x.Id == id && x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WorkoutImages = _context.WorkoutImages.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id).Select(i => new ImageDTO()
                    {
                        Id = i.Id,
                        WorkoutId = weightWorkout.Id,
                        ImageLarge = i.ImageLarge,
                        ImageSmall = i.ImageSmall,
                    }).ToList(),
                    WeightExercisesDto = _context.WeightExercises.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id && x.TotalExerciseWeight > 0.0).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.AsNoTracking().Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
                        MainMuscleGroup = _statFunctions.TryGetMuscle(_context.WeightActivities.Single(a => a.Id == x.ActivityId)),
                        WeightRoundsDto = _context.WeightRounds.AsNoTracking().Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise,
                            Color = r.Color
                        }).ToList()
                    }).ToList(),
                }).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{date}")]
        public IActionResult GetWeightWorkout([FromRoute] DateTime date)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                return Ok(_context.WeightWorkouts.AsNoTracking().Where(x => x.WorkoutDate.Year == date.Year && x.WorkoutDate.DayOfYear == date.DayOfYear && x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WorkoutImages = _context.WorkoutImages.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id).Select(i => new ImageDTO()
                    {
                        Id = i.Id,
                        WorkoutId = weightWorkout.Id,
                        ImageLarge = i.ImageLarge,
                        ImageSmall = i.ImageSmall,
                    }).ToList(),
                    WeightExercisesDto = _context.WeightExercises.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id && x.TotalExerciseWeight > 0.0).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.AsNoTracking().Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
                        MainMuscleGroup = _statFunctions.TryGetMuscle(_context.WeightActivities.Single(a => a.Id == x.ActivityId)),
                        WeightRoundsDto = _context.WeightRounds.AsNoTracking().Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise,
                            Color = r.Color
                        }).ToList()
                    }).ToList(),
                }).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{year}/{dayOfYear}")]
        public IActionResult GetWeightWorkout([FromRoute] int year, int dayOfYear)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_context.WeightWorkouts.AsNoTracking().Where(x => x.WorkoutDate.Year == year && x.WorkoutDate.DayOfYear == dayOfYear && x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WorkoutImages = _context.WorkoutImages.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id).Select(i => new ImageDTO()
                    {
                        Id = i.Id,
                        WorkoutId = weightWorkout.Id,
                        ImageLarge = i.ImageLarge,
                        ImageSmall = i.ImageSmall,
                    }).ToList(),
                    WeightExercisesDto = _context.WeightExercises.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id && x.TotalExerciseWeight > 0.0).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.AsNoTracking().Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
                        MainMuscleGroup = _statFunctions.TryGetMuscle(_context.WeightActivities.Single(a => a.Id == x.ActivityId)),
                        WeightRoundsDto = _context.WeightRounds.AsNoTracking().Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise,
                            Color = r.Color
                        }).ToList()
                    }).ToList(),
                }).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetWeightWorkoutByGuid/{guid}")]
        public IActionResult GetWeightWorkout([FromRoute] string guid)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                return Ok(_context.WeightWorkouts.AsNoTracking().Where(x => x.WorkoutGuid.ToString() == guid && x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WorkoutImages = _context.WorkoutImages.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id).Select(i => new ImageDTO()
                    {
                        Id = i.Id,
                        WorkoutId = weightWorkout.Id,
                        ImageLarge = i.ImageLarge,
                        ImageSmall = i.ImageSmall,
                    }).ToList(),
                    WeightExercisesDto = _context.WeightExercises.AsNoTracking().Where(x => x.WorkoutId == weightWorkout.Id && x.TotalExerciseWeight > 0.0).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.AsNoTracking().Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
                        MainMuscleGroup = _statFunctions.TryGetMuscle(_context.WeightActivities.Single(a => a.Id == x.ActivityId)),
                        WeightRoundsDto = _context.WeightRounds.AsNoTracking().Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise,
                            Color = r.Color
                        }).ToList()
                    }).ToList(),
                }).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("IsWeightWorkoutExistsByGuid/{guid}")]
        public IActionResult IsWeightWorkoutExists([FromRoute] string guid)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_context.WeightWorkouts.Any(x => x.OwnerUserName == user.UserName && x.WorkoutGuid.ToString() == guid));
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("IsWeightWorkoutExistsByDate/{date}")]
        public IActionResult IsWeightWorkoutExists([FromRoute] DateTime date)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_context.WeightWorkouts.Any(x => x.OwnerUserName == user.UserName && x.WorkoutDate.Year == date.Year && x.WorkoutDate.DayOfYear == date.DayOfYear));
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("IsWeightWorkoutExistsByYearAndDay/{year}/{dayOfYear}")]
        public IActionResult IsWeightWorkoutExists([FromRoute] int year, int dayOfYear)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_context.WeightWorkouts.Any(x => x.OwnerUserName == user.UserName && x.WorkoutDate.Year == year && x.WorkoutDate.DayOfYear == dayOfYear));
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/WeightWorkouts/5
        [HttpPut]
        public async Task<IActionResult> PutWeightWorkout([FromBody] WeightWorkoutDTO weightWorkoutDTO)
        {
            //var transaction = _context.Database.BeginTransaction();

            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                WeightWorkout weightWorkout = _context.WeightWorkouts.FirstOrDefault(x => x.Id == weightWorkoutDTO.Id && x.OwnerUserName == user.UserName);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (weightWorkout == null)
                    return NotFound();

                if (weightWorkout.Id != weightWorkoutDTO.Id)
                    return BadRequest();

                weightWorkout.WorkoutGuid = weightWorkoutDTO.WorkoutGuid;
                weightWorkout.WorkoutName = weightWorkoutDTO.WorkoutName;
                weightWorkout.Note = weightWorkoutDTO.Note;
                weightWorkout.TotalWeight = weightWorkoutDTO.TotalWeight;
                weightWorkout.WorkoutType = weightWorkoutDTO.WorkoutType;
                weightWorkout.WorkoutDate = new DateTime(weightWorkoutDTO.WorkoutDate.Date.Year, weightWorkoutDTO.WorkoutDate.Month, weightWorkoutDTO.WorkoutDate.Date.Day);

                List<WeightExercise> weightExercises = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkoutDTO.Id).ToList();

                foreach (var exercise in weightExercises)
                {
                    List<WeightRound> weightRounds = _context.WeightRounds.Where(x => x.ExerciseId == exercise.Id).ToList();

                    foreach (var round in weightRounds)
                    {
                        if (round == null)
                            return NotFound();

                        _context.WeightRounds.Remove(round);
                    }

                    if (exercise == null)
                        return NotFound();

                    _context.WeightExercises.Remove(exercise);
                }

                await _context.SaveChangesAsync();

                await AddWeightExercisesAsync(weightWorkoutDTO.WeightExercisesDto, weightWorkoutDTO.WorkoutGuid);

                if (weightWorkoutDTO.WorkoutImages != null)
                    await AddImagesForWorkoutAsync(weightWorkoutDTO.WorkoutImages, weightWorkoutDTO.WorkoutGuid);

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/WeightWorkouts
        [HttpPost]
        public async Task<IActionResult> PostWeightWorkout([FromBody] WeightWorkoutDTO weightWorkoutDTO)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newWeightWorkout = new WeightWorkout()
                {
                    WorkoutGuid = weightWorkoutDTO.WorkoutGuid,
                    WorkoutName = weightWorkoutDTO.WorkoutName,
                    Note = weightWorkoutDTO.Note,
                    TotalWeight = weightWorkoutDTO.TotalWeight,
                    WorkoutType = weightWorkoutDTO.WorkoutType,
                    WorkoutDate = new DateTime(weightWorkoutDTO.WorkoutDate.Date.Year, weightWorkoutDTO.WorkoutDate.Month, weightWorkoutDTO.WorkoutDate.Date.Day),
                    OwnerUserName = user.UserName,
                };

                var addedWorkout = _context.WeightWorkouts.Add(newWeightWorkout);
                await _context.SaveChangesAsync();
                weightWorkoutDTO.Id = addedWorkout.Entity.Id;

                if (weightWorkoutDTO.WorkoutImages != null)
                    await AddImagesForWorkoutAsync(weightWorkoutDTO.WorkoutImages, weightWorkoutDTO.WorkoutGuid);

                await AddWeightExercisesAsync(weightWorkoutDTO.WeightExercisesDto, weightWorkoutDTO.WorkoutGuid);
                await MergeMultipleActivities();

                return CreatedAtAction("GetWeightWorkout", new { id = addedWorkout.Entity.Id }, weightWorkoutDTO);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/WeightWorkouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightWorkout([FromRoute] int id)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var weightWorkout = _context.WeightWorkouts.Single(x => x.OwnerUserName == user.UserName && x.Id == id);

                if (weightWorkout == null)
                    return NotFound();

                if (_context.WorkoutImages.Any(x => x.OwnerUserName == user.UserName && x.WorkoutId == id))
                    await RemoveImages(weightWorkout.Id);

                await RemoveExercises(weightWorkout.Id);
                _context.WeightWorkouts.Remove(weightWorkout);
                await _context.SaveChangesAsync();

                return Ok(weightWorkout);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/WeightWorkouts/5
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteWeightWorkout([FromRoute] string guid)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var weightWorkout = _context.WeightWorkouts.Single(x => x.OwnerUserName == user.UserName && x.WorkoutGuid.ToString() == guid);

                if (weightWorkout == null)
                    return NotFound();

                if (_context.WorkoutImages.Any(x => x.OwnerUserName == user.UserName && x.WorkoutId == weightWorkout.Id))
                    await RemoveImages(weightWorkout.Id);

                await RemoveExercises(weightWorkout.Id);
                _context.WeightWorkouts.Remove(weightWorkout);
                await _context.SaveChangesAsync();

                return Ok(weightWorkout);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("MovedWeightsByMonth")]
        public IActionResult GetMovedWeightsByMonth()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_statFunctions.SumMovedWeightsByMonth(_context.WeightWorkouts.Where(x => x.OwnerUserName == user.UserName)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetMaxMovedWeightsByActivites")]
        public IActionResult GetMaxMovedWeightsByActivites()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_statFunctions.FindMaxMovedWeightsByActivites(
                    _context.WeightExercises.Where(u => u.OwnerUserName == user.UserName),
                    _context.WeightActivities.Where(u => u.OwnerUserName == user.UserName)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("MovedWeightsInTheMonth/{year}/{month}")]
        public IActionResult GetMovedWeightsInTheMonth([FromRoute] int year, int month)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_statFunctions.CollectMovedWeightsInTheMonth(_context.WeightWorkouts.Where(x => x.OwnerUserName == user.UserName), year, month));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetMovedWeightsGroupByMonth")]
        public IActionResult GetMovedWeightsGroupByMonth()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_statFunctions.CollectMovedWeightsGroupByMonth(_context.WeightWorkouts.Where(x => x.OwnerUserName == user.UserName)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetRecentWorkouts")]
        public IActionResult GetRecentWorkouts()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(GetUserWorkouts(user).OrderByDescending(x => x.WorkoutDate.Date).Where(w => w.WorkoutDate < DateTime.Now).Take(5));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetRangeOfHistoryItems/{batch}/{number}")]
        public IActionResult GetRecentWorkouts([FromRoute] int batch, int number)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(GetHistoryItemDTOs((WeightWorkout x) => x.OwnerUserName == user.UserName && x.WorkoutDate.Year <= DateTime.Now.Year && x.WorkoutDate.DayOfYear <= DateTime.Now.DayOfYear)
                    .OrderByDescending(x => x.WorkoutDate.Date)
                    .Skip(batch)
                    .Take(number));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetCalendarItemsInMonth/{year}/{month}")]
        public IActionResult GetCalendarItems([FromRoute] int year, int month)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(GetCalendarItemDTOs((WeightWorkout x) => x.OwnerUserName == user.UserName && x.WorkoutDate.Year == year && x.WorkoutDate.Month == month)
                    .OrderByDescending(x => x.WorkoutDate.Date));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetThisweekWeightsByMuscle")]
        public IActionResult GetThisweekWeightsByMuscle()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_statFunctions.CollectRecentMovedWeightsGroupByMuscle(
                    _context.WeightWorkouts.Where(u => u.OwnerUserName == user.UserName),
                    _context.WeightExercises.Where(u => u.OwnerUserName == user.UserName),
                    _context.WeightActivities.Where(u => u.OwnerUserName == user.UserName)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("SearchWorkout/{keyWords}")]
        public IActionResult SearchInWorkouts([FromRoute] string keyWords)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var allAvailableWorkouts = GetUserWorkouts(user);

                if (!string.IsNullOrEmpty(keyWords))
                {
                    string[] searchStrings = keyWords.Trim().Split(' ');
                    var foundElements = searchStrings.SelectMany(str => allAvailableWorkouts.Where(x => x.WorkoutName.ToUpper().Contains(str.ToUpper()) || x.WeightExercisesDto.Any(ex => ex.ExerciseName.ToUpper().Contains(str.ToUpper()))));
                    List<WeightWorkoutDTO> uniqueFoundElements = new List<WeightWorkoutDTO>();

                    foreach (var item in foundElements)
                    {
                        if (!uniqueFoundElements.Any(x => x.WorkoutGuid == item.WorkoutGuid))
                            uniqueFoundElements.Add(item);
                    }

                    return Ok(uniqueFoundElements.OrderByDescending(x => x.WorkoutDate.Date));
                }
                else
                    return Ok(allAvailableWorkouts.OrderByDescending(x => x.WorkoutDate.Date));

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task RemoveExercises(int id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var exercises = _context.WeightExercises.Where(ex => ex.OwnerUserName == user.UserName && ex.WorkoutId == id);

            foreach (var exercise in exercises)
            {
                await RemoveRounds(exercise.Id);
                _context.WeightExercises.Remove(exercise);
            }

            await _context.SaveChangesAsync();
        }

        private async Task RemoveRounds(int id)
        {
            var rounds = _context.WeightRounds.Where(ex => ex.ExerciseId == id);

            foreach (var round in rounds)
            {
                await RemoveRounds(round.Id);
                _context.WeightRounds.Remove(round);
            }

            await _context.SaveChangesAsync();
        }

        private bool WeightWorkoutExists(int id)
        {
            return _context.WeightWorkouts.Any(e => e.Id == id);
        }

        private async Task<bool> AddWeightExercisesAsync(ICollection<WeightExerciseDTO> weightExercisesDto, Guid workoutGuid)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                foreach (var weightExerciseDto in weightExercisesDto)
                {
                    int actId = AddActivityByNameIfNeeded(weightExerciseDto.ExerciseName, weightExerciseDto.MainMuscleGroup);

                    var weightExercise = new WeightExercise()
                    {
                        ExerciseGuid = weightExerciseDto.ExerciseGuid,
                        ActivityId = actId,
                        Note = weightExerciseDto.Note,
                        TotalExerciseWeight = weightExerciseDto.TotalExerciseWeight,
                        WorkoutId = _context.WeightWorkouts.Single(x => x.WorkoutGuid == workoutGuid).Id,
                        OwnerUserName = user.UserName,
                        Color = weightExerciseDto.Color
                    };

                    var addedweightExercise = _context.WeightExercises.Add(weightExercise);
                    await _context.SaveChangesAsync();
                    await AddWeightRoundAsync(weightExerciseDto.WeightRoundsDto, weightExerciseDto.ExerciseGuid);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private int AddActivityByNameIfNeeded(string exerciseName, Muscle mainMuscleGroup)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var trimmedExerciseName = exerciseName.Trim();

            if (_context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Any(x => x.ActivityName.ToUpper() == trimmedExerciseName.ToUpper()))
            {
                //Ha már szerepel az adatbázisban, akkor újra elmentjük a muscle-t
                var activity = _context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Single(x => x.ActivityName.ToUpper() == trimmedExerciseName.ToUpper());
                activity.MainMuscleGroup = mainMuscleGroup;
                var addedActivity = _context.WeightActivities.Update(activity);
                _context.SaveChanges();
                return activity.Id;
            }
            else
            {
                var newActivity = new WeightActivity()
                {
                    ActivityGuid = Guid.NewGuid(),
                    ActivityName = trimmedExerciseName,
                    OwnerUserName = user.UserName,
                    MainMuscleGroup = mainMuscleGroup,
                };

                var addedActivity = _context.WeightActivities.Add(newActivity);
                _context.SaveChanges();
                return addedActivity.Entity.Id;
            }
        }

        private async Task<bool> AddWeightRoundAsync(ICollection<WeightRoundDTO> weightRoundsDto, Guid exerciseGuid)
        {
            try
            {
                foreach (var weightRoundDto in weightRoundsDto)
                {
                    var weightRound = new WeightRound()
                    {
                        Reps = weightRoundDto.Reps,
                        RoundGuid = weightRoundDto.RoundGuid,
                        RoundNumber = weightRoundDto.RoundNumber,
                        WeightOfExercise = weightRoundDto.WeightOfExercise,
                        ExerciseId = _context.WeightExercises.Single(x => x.ExerciseGuid == exerciseGuid).Id,
                        Color = weightRoundDto.Color
                    };

                    var addedWeightRound = _context.WeightRounds.Add(weightRound);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("Merge")]
        public async Task<IActionResult> CommonMergeMultipleActivities()
        {
            try
            {
                await MergeMultipleActivities();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        private Task MergeMultipleActivities()
        {
            return Task.Run(() =>
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var userActivities = _context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).ToArray();

                List<string> activityIds = new List<string>();

                for (int i = 0; i < userActivities.Count(); i++)
                {
                    if (userActivities.Count(x => x.ActivityName.ToUpper().Trim() == userActivities[i].ActivityName.ToUpper().Trim()) > 1 &&
                        !activityIds.Any(x => x.ToUpper().Trim() == userActivities[i].ActivityName.ToUpper().Trim()))
                    {
                        activityIds.Add(userActivities[i].ActivityName.ToUpper().Trim());
                    }
                }

                foreach (var activityId in activityIds)
                {
                    var exercisesToRemove = userActivities.Where(x => x.ActivityName.ToUpper().Trim() == activityId).ToList();
                    int index = 0;

                    //keep the first instance
                    var original = exercisesToRemove[index];
                    ++index;

                    while (index <= exercisesToRemove.Count() - 1)
                    {
                        if (exercisesToRemove[index].IsWatched)
                        {
                            original.IsWatched = true;
                            _context.SaveChanges();
                        }

                        if (exercisesToRemove[index].MainMuscleGroup != Muscle.Unknown)
                        {
                            original.MainMuscleGroup = exercisesToRemove[index].MainMuscleGroup;
                            _context.SaveChanges();
                        }

                        if (_context.WeightExercises.Any(x => x.ActivityId == exercisesToRemove[index].Id))
                        {
                            var exercises = _context.WeightExercises.Where(x => x.ActivityId == exercisesToRemove[index].Id);

                            foreach (var item in exercises)
                                item.ActivityId = original.Id;
                        }

                        _context.WeightActivities.Remove(exercisesToRemove[index]);
                        _context.SaveChanges();
                        ++index;
                    }
                }
            });
        }

        private async Task AddImagesForWorkoutAsync(ICollection<ImageDTO> workoutImages, Guid workoutGuid)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            foreach (var imageDTO in workoutImages)
            {
                var image = new WorkoutImage()
                {
                    ImageLarge = imageDTO.ImageLarge,
                    ImageSmall = imageDTO.ImageSmall,
                    WorkoutId = _context.WeightWorkouts.Single(x => x.WorkoutGuid == workoutGuid).Id,
                    OwnerUserName = user.UserName
                };

                var addedWorkout = _context.WorkoutImages.Add(image);
                await _context.SaveChangesAsync();
            }
        }

        private async Task RemoveImages(int id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var images = _context.WorkoutImages.Where(ex => ex.OwnerUserName == user.UserName && ex.WorkoutId == id);

            foreach (var image in images)
            {
                _context.WorkoutImages.Remove(image);
            }

            await _context.SaveChangesAsync();
        }

        private IEnumerable<WeightWorkoutDTO> GetUserWorkouts(ApplicationUser user)
        {
            return _context.WeightWorkouts.Where(x => x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
            {
                Id = weightWorkout.Id,
                WorkoutName = weightWorkout.WorkoutName,
                WorkoutGuid = weightWorkout.WorkoutGuid,
                Note = weightWorkout.Note,
                TotalWeight = weightWorkout.TotalWeight,
                WorkoutType = weightWorkout.WorkoutType,
                WorkoutDate = weightWorkout.WorkoutDate,
                WorkoutImages = _context.WorkoutImages.Where(x => x.WorkoutId == weightWorkout.Id).Select(i => new ImageDTO()
                {
                    Id = i.Id,
                    WorkoutId = weightWorkout.Id,
                    ImageLarge = i.ImageLarge,
                    ImageSmall = i.ImageSmall,
                }).ToList(),
                WeightExercisesDto = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkout.Id && x.TotalExerciseWeight > 0.0).Select(x => new WeightExerciseDTO()
                {
                    ExerciseGuid = x.ExerciseGuid,
                    Id = x.Id,
                    ExerciseName = _context.WeightActivities.Single(a => a.Id == x.ActivityId).ActivityName,
                    Note = x.Note,
                    TotalExerciseWeight = x.TotalExerciseWeight,
                    Color = x.Color,
                    MainMuscleGroup = _statFunctions.TryGetMuscle(_context.WeightActivities.Single(a => a.Id == x.ActivityId)),
                    WeightRoundsDto = _context.WeightRounds.Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                    {
                        Id = r.Id,
                        Reps = r.Reps,
                        RoundGuid = r.RoundGuid,
                        RoundNumber = r.RoundNumber,
                        WeightOfExercise = r.WeightOfExercise,
                        Color = r.Color
                    }).ToList()
                }).ToList(),
            });
        }

        private IEnumerable<HistoryItemDTO> GetHistoryItemDTOs(Func<WeightWorkout, bool> predicate) =>
            _context.WeightWorkouts
                    .Where(x => predicate.Invoke(x))
                    .Select(weightWorkout => new HistoryItemDTO()
                    {
                        WorkoutName = weightWorkout.WorkoutName,
                        WorkoutGuid = weightWorkout.WorkoutGuid,
                        TotalWeight = weightWorkout.TotalWeight,
                        WorkoutDate = weightWorkout.WorkoutDate,
                    });

        private IEnumerable<CalendarItemDTO> GetCalendarItemDTOs(Func<WeightWorkout, bool> predicate) =>
            _context.WeightWorkouts
                    .Where(x => predicate.Invoke(x))
                    .Select(weightWorkout => new CalendarItemDTO()
                    {
                        WorkoutGuid = weightWorkout.WorkoutGuid,
                        WorkoutDate = weightWorkout.WorkoutDate,
                    });
    }
}