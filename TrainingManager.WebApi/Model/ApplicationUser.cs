using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainingManager.WebApi.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
    }
}
