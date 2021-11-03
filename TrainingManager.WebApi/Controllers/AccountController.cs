using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TrainingManagerContext _context;

        public AccountController(TrainingManagerContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userLoginDTO.UserName, userLoginDTO.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                    return Ok();

                ModelState.AddModelError("", "Acess denied!");
                return Unauthorized();
            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var profilePicture = System.IO.File.OpenRead("Images\\accountCircle.png");
                    await profilePicture.CopyToAsync(memoryStream);
                    byte[] profilePictureArray = memoryStream.ToArray();

                    var newUser = new ApplicationUser()
                    {
                        Name = userRegistrationDTO.Name,
                        UserName = userRegistrationDTO.UserName,
                        Email = userRegistrationDTO.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ProfilePicture = profilePictureArray,
                    };

                    var result = await _userManager.CreateAsync(newUser, userRegistrationDTO.Password);

                    return result.Succeeded ? Ok() : throw new Exception();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Profile")]
        public async Task<IActionResult> PostProfilePicture([FromBody] byte[] image)
        {
            try
            {
                ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (image == null || user == null)
                    return NotFound();

                user.ProfilePicture = image;
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Internal Server Error
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("OriginalProfile")]
        public async Task<IActionResult> GetProfilePicture()
        {
            try
            {
                ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                return Ok(user.ProfilePicture);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("NameOfTheUser")]
        public async Task<IActionResult> GetNameOfTheUser()
        {
            try
            {
                ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                return Ok(user.Name);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("Signout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}