using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TrainingManager.WebApi.Model
{
    public class PersonalRecord
    {
        public int Id { get; set; }
        public Guid PersonalRecordGuid { get; set; }
        public int ActivityId { get; set; }
        public Guid ActivityGuid { get; set; }
        public DateTime PersonalRecordDate { get; set; }
        public double WeightOfPersonalRecord { get; set; }
        public int RepsOfPersonalRecord { get; set; }

        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public string OwnerUserName { get; set; }
    }
}
