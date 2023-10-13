using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LiftIt.WebApi.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
