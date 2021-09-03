using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManager.Data.DTO;
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
        public IActionResult GetWeightWorkouts()
        {
            try
            {
                return Ok(_context.WeightWorkouts.Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WeightExercisesDto = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkout.Id).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = x.ExerciseName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = _context.WeightRounds.Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise
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

                return Ok(_context.WeightWorkouts.Where(x => x.Id == id).Select(weightWorkout => new WeightWorkoutDTO
                {
                    Id = weightWorkout.Id,
                    WorkoutName = weightWorkout.WorkoutName,
                    WorkoutGuid = weightWorkout.WorkoutGuid,
                    Note = weightWorkout.Note,
                    TotalWeight = weightWorkout.TotalWeight,
                    WorkoutType = weightWorkout.WorkoutType,
                    WorkoutDate = weightWorkout.WorkoutDate,
                    WeightExercisesDto = _context.WeightExercises.Where(x => x.WorkoutId == weightWorkout.Id).Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        Id = x.Id,
                        ExerciseName = x.ExerciseName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = _context.WeightRounds.Where(r => r.ExerciseId == x.Id).Select(r => new WeightRoundDTO()
                        {
                            Id = r.Id,
                            Reps = r.Reps,
                            RoundGuid = r.RoundGuid,
                            RoundNumber = r.RoundNumber,
                            WeightOfExercise = r.WeightOfExercise
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
                WeightWorkout weightWorkout = _context.WeightWorkouts.FirstOrDefault(x => x.Id == weightWorkoutDTO.Id);

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
                };

                var addedWorkout = _context.WeightWorkouts.Add(newWeightWorkout);
                await _context.SaveChangesAsync();
                weightWorkoutDTO.Id = addedWorkout.Entity.Id;
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var weightWorkout = await _context.WeightWorkouts.FindAsync(id);

                if (weightWorkout == null)
                    return NotFound();

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

        private bool WeightWorkoutExists(int id)
        {
            return _context.WeightWorkouts.Any(e => e.Id == id);
        }

        private async Task<bool> AddWeightExercisesAsync(ICollection<WeightExerciseDTO> weightExercisesDto, Guid workoutGuid)
        {
            try
            {
                foreach (var weightExerciseDto in weightExercisesDto)
                {
                    var weightExercise = new WeightExercise()
                    {
                        ExerciseGuid = weightExerciseDto.ExerciseGuid,
                        ExerciseName = weightExerciseDto.ExerciseName,
                        Note = weightExerciseDto.Note,
                        TotalExerciseWeight = weightExerciseDto.TotalExerciseWeight,
                        WorkoutId = _context.WeightWorkouts.Single(x => x.WorkoutGuid == workoutGuid).Id,
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
    }
}