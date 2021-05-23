using System;
using System.Collections.Generic;

namespace TrainingManager.Data.DTO
{
    public class RoundDTO
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public string RoundName { get; set; }
        public string Note { get; set; }
        public int Reps { get; set; }
        public int WorkoutId { get; set; }
        public ICollection<WeightDrillDTO> WeightDrills { get; set; }
    }
}
