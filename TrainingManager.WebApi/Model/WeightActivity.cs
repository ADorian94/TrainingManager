using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class WeightActivity
    {
        public int Id { get; set; }
        public Guid ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string OwnerUserName { get; set; }
    }
}
