using System.ComponentModel.DataAnnotations.Schema;

namespace LiftIt.WebApi.Model
{
    public class WorkoutImage
    {
        public int Id { get; set; }
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public byte[] ImageSmall { get; set; }
        public byte[] ImageLarge { get; set; }
        public string OwnerUserName { get; set; }
    }
}
