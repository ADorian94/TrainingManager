using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Data.DTO
{
    public class UserRegistrationDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
