using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Data.DTO
{
    public class WeightRoundDTO
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public int RoundNumber { get; set; }
        public double WeightOfDrill { get; set; }
        public int Reps { get; set; }
    }
}
