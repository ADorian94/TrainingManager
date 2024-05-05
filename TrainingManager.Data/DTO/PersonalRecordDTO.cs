using System;

namespace TrainingManager.Data.DTO
{
    public class PersonalRecordDTO
    {
        public int Id { get; set; }
        public Guid PersonalRecordGuid { get; set; }
        public string OwnerUserName { get; set; }
        public double WeightOfPersonalRecord { get; set; }
        public int RepsOfPersonalRecord { get; set; }
        public int ActivityId { get; set; }
        public int WorkoutId { get; set; }
        public DateTime PersonalRecordDate { get; set; }
    }
}
