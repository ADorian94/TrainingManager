using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Data.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public byte[] ImageSmall { get; set; }
        public byte[] ImageLarge { get; set; }
    }
}
