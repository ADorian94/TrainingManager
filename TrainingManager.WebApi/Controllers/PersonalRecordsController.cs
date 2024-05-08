using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions.Interfaces;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalRecordsController : ControllerBase
    {
        private readonly TrainingManagerContext _context;
        private readonly IPersonalRecordHelperFunctions _personalRecordFunctions;

        public PersonalRecordsController(TrainingManagerContext context, IPersonalRecordHelperFunctions personalRecordFunctions)
        {
            _context = context;
            _personalRecordFunctions = personalRecordFunctions;
        }

        // GET: api/PersonalRecords
        [HttpGet]
        public async Task<IActionResult> GetPersonalRecords()
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var activityGroups = _context.PersonalRecords.Where(u => u.OwnerUserName == user.UserName).GroupBy(x => x.ActivityId);
            return Ok(activityGroups.Select(x => CreatePersonalRecordDTO(x.OrderBy(y => y.PersonalRecordDate).FirstOrDefault())));
        }

        // GET: api/PersonalRecordHistory
        [HttpGet("PersonalRecordHistories")]
        public async Task<IActionResult> PersonalRecordHistories()
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            return Ok(_context.PersonalRecords.Where(u => u.OwnerUserName == user.UserName).Select(x => CreatePersonalRecordDTO(x)));
        }

        // GET: api/PersonalRecords/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalRecord([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var personalRecordDTO = _personalRecordFunctions.FindMaxMovedWeightsOfActivity(_context.PersonalRecords.Where(u => u.OwnerUserName == User.Identity.Name).FirstOrDefault(x => x.ActivityGuid == id).ActivityId);

            if (personalRecordDTO == null)
                return NotFound();

            return Ok(personalRecordDTO);
        }

        // GET: api/PersonalRecords/5
        [HttpGet("GetPersonalRecordHistory/{activityGuid}")]
        public async Task<IActionResult> GetPersonalRecordHistoryById([FromRoute] Guid activityGuid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var personalRecords = _context.PersonalRecords.Where(x => x.OwnerUserName == user.UserName && x.ActivityGuid == activityGuid).Select(r => CreatePersonalRecordDTO(r));

            if (personalRecords == null)
                return NotFound();

            return Ok(personalRecords);
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