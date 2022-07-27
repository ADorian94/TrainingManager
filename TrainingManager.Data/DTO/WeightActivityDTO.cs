using System;

namespace TrainingManager.Data.DTO
{
    public class WeightActivityDTO
    {
        public Guid ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public Muscle MainMuscleGroup { get; set; }
        public bool IsWatched { get; set; }
    }
}
