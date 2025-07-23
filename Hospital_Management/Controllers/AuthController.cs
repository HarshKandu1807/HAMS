using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Iuser iuser;
        public AuthController(Iuser iuser)
        {
            this.iuser = iuser;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await iuser.Register(userDTO);
            if (user == null)
            {
                return BadRequest("User Already exist");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await iuser.Login(loginDTO);

            if (result == "NotFound")
            {
                return NotFound("User Not Found");
            }
            else if (result == "Incorrect")
            {
                return BadRequest("Incorrect Credentials");
            }
            return Ok(result);
        }
    }
}
