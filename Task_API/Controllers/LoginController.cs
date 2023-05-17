using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTRepository _jwtRepository;

        public LoginController(IJWTRepository jwtRepository, IUserRepository userRepository)
        {
            _jwtRepository = jwtRepository;
            _userRepository = userRepository;
        }



        [HttpPost("Login")]
        public async Task<ActionResult<MLogin>> login(MLogin mLogin)
        {
            try
            {
                var user = await _userRepository.GetUserByName(mLogin.UName);

                if (mLogin == null || string.IsNullOrEmpty(user.UName) || string.IsNullOrEmpty(user.UPassword))
                {
                    return BadRequest("Invalid username or password");
                }
                var user1 = await _userRepository.GetUserByName(user.UName);
                var userindata = await _userRepository.GetUserByName(user1.UName);
                var hashedPassword = await _jwtRepository.ValidateHashingPasswordAsync(mLogin.UPassword);

                if (userindata.UPassword != hashedPassword.UPassword)
                {
                    return BadRequest("Invalid Username or password");
                }
                var Token = await _jwtRepository.TokenGenerate(user1);
                return Ok(Token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Servar Error");
            }

        }
    }
}
