using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Data.DTO
{
    public class YearMonthWorkoutGroupDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public IEnumerable<MovedWeightsInMonthDTO> WorkoutsInMonth { get; set; }
    }
}
