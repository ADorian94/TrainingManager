using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
#warning Refaktorálni
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeightWorkoutsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public WeightWorkoutsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/WeightWorkouts
        [HttpGet]
        public IActionResult GetWeightWorkouts()
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return Ok(_context.WeightWorkouts.Where(x => x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
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
                    WeightExercisesDto = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkout.Id).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
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
                }));
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

                return Ok(_context.WeightWorkouts.Where(x => x.Id == id && x.OwnerUserName == user.UserName).Select(weightWorkout => new WeightWorkoutDTO
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
                    WeightExercisesDto = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkout.Id).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = _context.WeightActivities.Single(a => a.Id == x.ActivityId).ActivityName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        Color = x.Color,
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
                }).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/WeightWorkouts/5
        [HttpPut]
        public async Task<IActionResult> PutWeightWorkout([FromBody] WeightWorkoutDTO weightWorkoutDTO)
        {
            try
            {
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                WeightWorkout weightWorkout = _context.WeightWorkouts.FirstOrDefault(x => x.Id == weightWorkoutDTO.Id && x.OwnerUserName == user.UserName);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (weightWorkout.Id != weightWorkoutDTO.Id)
                    return BadRequest();

                if (weightWorkout == null)
                    return NotFound();

                weightWorkout.WorkoutGuid = weightWorkoutDTO.WorkoutGuid;
                weightWorkout.WorkoutName = weightWorkoutDTO.WorkoutName;
                weightWorkout.Note = weightWorkoutDTO.Note;
                weightWorkout.TotalWeight = weightWorkoutDTO.TotalWeight;
                weightWorkout.WorkoutType = weightWorkoutDTO.WorkoutType;
                weightWorkout.WorkoutDate = weightWorkoutDTO.WorkoutDate;

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

                if (weightWorkoutDTO.WorkoutImages != null)
                    await AddImagesForWorkoutAsync(weightWorkoutDTO.WorkoutImages, weightWorkoutDTO.WorkoutGuid);

                await AddWeightExercisesAsync(weightWorkoutDTO.WeightExercisesDto, weightWorkoutDTO.WorkoutGuid);
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
                    WorkoutDate = weightWorkoutDTO.WorkoutDate,
                    OwnerUserName = user.UserName,
                };

                var addedWorkout = _context.WeightWorkouts.Add(newWeightWorkout);
                await _context.SaveChangesAsync();
                weightWorkoutDTO.Id = addedWorkout.Entity.Id;

                if (weightWorkoutDTO.WorkoutImages != null)
                    await AddImagesForWorkoutAsync(weightWorkoutDTO.WorkoutImages, weightWorkoutDTO.WorkoutGuid);

                await AddWeightExercisesAsync(weightWorkoutDTO.WeightExercisesDto, weightWorkoutDTO.WorkoutGuid);

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
                    int actId = AddActivityByNameIfNeeded(weightExerciseDto.ExerciseName);

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

        private int AddActivityByNameIfNeeded(string exerciseName)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (_context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Any(x => x.ActivityName.ToUpper() == exerciseName.ToUpper()))
            {
                return _context.WeightActivities.Where(x => x.OwnerUserName == user.UserName).Single(x => x.ActivityName.ToUpper() == exerciseName.ToUpper()).Id;
            }
            else
            {
                var newActivity = new WeightActivity()
                {
                    ActivityGuid = Guid.NewGuid(),
                    ActivityName = exerciseName,
                    OwnerUserName = user.UserName,
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
    }
}