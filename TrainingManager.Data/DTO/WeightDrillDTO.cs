using System;

namespace TrainingManager.Data.DTO
{
    public class WeightDrillDTO
    {
        public int Id { get; set; }
        public Guid DrillGuid { get; set; }
        public string DrillName { get; set; }
        public string Note { get; set; }
        public double WeightOfDrill { get; set; }
        public DateTime DrillDate { get; set; }
        public int Reps { get; set; }
        public int WorkoutId { get; set; }
        public int RoundId { get; set; }
    }
}
