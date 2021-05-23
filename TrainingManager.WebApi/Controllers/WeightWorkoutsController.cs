using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    Rounds = _context.WeightRounds.Where(x => x.WorkoutId == weightWorkout.Id).Select(round => new RoundDTO
                    {
                        Id = round.Id,
                        RoundGuid = round.RoundGuid,
                        RoundName = round.RoundName,
                        WorkoutId = round.WorkoutId,
                        Reps = round.Reps,
                        Note = round.Note,
                        WeightDrills = _context.WeightDrills.Where(x => x.RoundId == round.Id).Select(drill => new WeightDrillDTO
                        {
                            Id = drill.Id,
                            DrillName = drill.DrillName,
                            DrillGuid = drill.DrillGuid,
                            RoundId = drill.RoundId,
                            DrillDate = drill.DrillDate,
                            Note = drill.Note,
                            Reps = drill.Reps,
                            WorkoutId = drill.WorkoutId,
                            WeightOfDrill = drill.WeightOfDrill
                        }).ToList()
                    }).ToList(),
                    WeightDrills = _context.WeightDrills.Where(x => x.WorkoutId == weightWorkout.Id).Select(drill => new WeightDrillDTO
                    {
                        Id = drill.Id,
                        DrillName = drill.DrillName,
                        DrillGuid = drill.DrillGuid,
                        RoundId = drill.RoundId,
                        DrillDate = drill.DrillDate,
                        Note = drill.Note,
                        Reps = drill.Reps,
                        WorkoutId = drill.WorkoutId,
                        WeightOfDrill = drill.WeightOfDrill
                    }).ToList()
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
                    Rounds = _context.WeightRounds.Where(x => x.WorkoutId == weightWorkout.Id).Select(round => new RoundDTO
                    {
                        Id = round.Id,
                        RoundGuid = round.RoundGuid,
                        RoundName = round.RoundName,
                        WorkoutId = round.WorkoutId,
                        Reps = round.Reps,
                        Note = round.Note,
                        WeightDrills = _context.WeightDrills.Where(x => x.RoundId == round.Id).Select(drill => new WeightDrillDTO
                        {
                            Id = drill.Id,
                            DrillName = drill.DrillName,
                            DrillGuid = drill.DrillGuid,
                            RoundId = drill.RoundId,
                            DrillDate = drill.DrillDate,
                            Note = drill.Note,
                            Reps = drill.Reps,
                            WorkoutId = drill.WorkoutId,
                            WeightOfDrill = drill.WeightOfDrill
                        }).ToList()
                    }).ToList(),
                    WeightDrills = _context.WeightDrills.Where(x => x.WorkoutId == weightWorkout.Id).Select(drill => new WeightDrillDTO
                    {
                        Id = drill.Id,
                        DrillName = drill.DrillName,
                        DrillGuid = drill.DrillGuid,
                        RoundId = drill.RoundId,
                        DrillDate = drill.DrillDate,
                        Note = drill.Note,
                        Reps = drill.Reps,
                        WorkoutId = drill.WorkoutId,
                        WeightOfDrill = drill.WeightOfDrill
                    }).ToList()
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightWorkout([FromRoute] int id, [FromBody] WeightWorkoutDTO weightWorkoutDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != weightWorkoutDTO.Id)
                    return BadRequest();

                WeightWorkout weightWorkout = _context.WeightWorkouts.FirstOrDefault(x => x.Id == weightWorkoutDTO.Id);

                if (weightWorkout == null)
                    return NotFound();

                weightWorkout.WorkoutGuid = weightWorkoutDTO.WorkoutGuid;
                weightWorkout.WorkoutName = weightWorkoutDTO.WorkoutName;
                weightWorkout.Note = weightWorkoutDTO.Note;
                weightWorkout.TotalWeight = weightWorkoutDTO.TotalWeight;
                weightWorkout.WorkoutType = weightWorkoutDTO.WorkoutType;
                weightWorkout.WorkoutDate = weightWorkoutDTO.WorkoutDate;

                //todo: rounds és drills tárolása

                await _context.SaveChangesAsync();
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
                weightWorkoutDTO.Id = addedWorkout.Entity.Id;
                AddWeightDrill(weightWorkoutDTO.WeightDrills);
                AddRounds(weightWorkoutDTO.Rounds);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetWeightWorkout", new { id = addedWorkout.Entity.Id }, weightWorkoutDTO);
            }
            catch
            {
                // Internal Server Error
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

        private bool AddRounds(ICollection<RoundDTO> roundDTOs)
        {
            try
            {
                foreach (var roundDTO in roundDTOs)
                {
                    var newRound = new Round()
                    {
                        RoundName = roundDTO.RoundName,
                        RoundGuid = roundDTO.RoundGuid,
                        Note = roundDTO.Note,
                        Reps = roundDTO.Reps,
                        WorkoutId = roundDTO.WorkoutId,
                    };

                    AddWeightDrill(roundDTO.WeightDrills);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool AddWeightDrill(ICollection<WeightDrillDTO> weightDrillDTOs)
        {
            try
            {
                foreach (var weightDrillDTO in weightDrillDTOs)
                {
                    var newWeightDrill = new WeightDrill()
                    {
                        DrillName = weightDrillDTO.DrillName,
                        DrillDate = weightDrillDTO.DrillDate,
                        DrillGuid = weightDrillDTO.DrillGuid,
                        Note = weightDrillDTO.Note,
                        Reps = weightDrillDTO.Reps,
                        RoundId = weightDrillDTO.RoundId,
                        WeightOfDrill = weightDrillDTO.WeightOfDrill,
                        WorkoutId = weightDrillDTO.WorkoutId
                    };

                    var addedDrill = _context.WeightDrills.Add(newWeightDrill);
                    weightDrillDTO.Id = addedDrill.Entity.Id;
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