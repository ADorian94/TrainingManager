using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalRecordsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;

        public PersonalRecordsController(TrainingManagerContext context)
        {
            _context = context;
        }

        // GET: api/PersonalRecords
        [HttpGet]
        public IActionResult GetPersonalRecords()
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var activityGroups = _context.PersonalRecords.Where(u => u.OwnerUserName == user.UserName).GroupBy(x => x.ActivityId);
            return Ok(activityGroups.Select(x => CreatePersonalRecordDTO(x.OrderBy(y => y.PersonalRecordDate).FirstOrDefault())));
        }

        // GET: api/PersonalRecordHistory
        [HttpGet("PersonalRecordHistories")]
        public IActionResult PersonalRecordHistories()
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            return Ok(_context.PersonalRecords.Where(u => u.OwnerUserName == user.UserName).Select(x => CreatePersonalRecordDTO(x)));
        }

        // GET: api/PersonalRecords/5
        [HttpGet("{id}")]
        public IActionResult GetPersonalRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var personalRecord = _context.PersonalRecords.Where(u => u.OwnerUserName == User.Identity.Name).Single(x => x.Id == id);

            if (personalRecord == null)
                return NotFound();

            return Ok(CreatePersonalRecordDTO(personalRecord));
        }

        // GET: api/PersonalRecords/5
        [HttpGet("GetPersonalRecordHistory/{id}")]
        public IActionResult GetPersonalRecordHistory([FromRoute] int activityId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var personalRecords = _context.PersonalRecords.Where(x => x.OwnerUserName == user.UserName && x.ActivityId == activityId).Select(r => CreatePersonalRecordDTO(r));

            if (personalRecords == null)
                return NotFound();

            return Ok(personalRecords);
        }

        // DELETE: api/PersonalRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personalRecord = await _context.PersonalRecords.FindAsync(id);

            if (personalRecord == null)
                return NotFound();

            _context.PersonalRecords.Remove(personalRecord);
            await _context.SaveChangesAsync();

            return Ok(personalRecord);
        }

        private PersonalRecordDTO CreatePersonalRecordDTO(PersonalRecord record) => new PersonalRecordDTO()
        {
            Id = record.Id,
            ActivityId = record.ActivityId,
            WorkoutId = record.WorkoutId,
            OwnerUserName = record.OwnerUserName,
            PersonalRecordDate = record.PersonalRecordDate,
            PersonalRecordGuid = record.PersonalRecordGuid,
            WeightOfPersonalRecord = record.WeightOfPersonalRecord,
            RepsOfPersonalRecord = record.RepsOfPersonalRecord
        };
    }
}