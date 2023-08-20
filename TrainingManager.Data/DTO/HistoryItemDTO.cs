using System;

namespace TrainingManager.Data.DTO
{
    public class HistoryItemDTO
    {
        public string WorkoutName { get; set; }
        public double TotalWeight { get; set; }
        public DateTime WorkoutDate { get; set; }
        public Guid WorkoutGuid { get; set; }
    }
}
