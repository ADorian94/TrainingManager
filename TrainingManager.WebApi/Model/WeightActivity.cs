using System;
using TrainingManager.Data;

namespace TrainingManager.WebApi.Model
{
    public class WeightActivity
    {
        public int Id { get; set; }
        public Guid ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public bool IsWatched { get; set; }
        public Muscle MainMuscleGroup { get; set; }
        public string OwnerUserName { get; set; }
    }
}
