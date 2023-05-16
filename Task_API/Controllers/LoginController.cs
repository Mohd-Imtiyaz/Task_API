using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        TaskDataBaseContext taskDataBaseContext;
        private readonly IUserRepository _userRepository;
        private readonly IJWTRepository _jwtRepository;

        public LoginController(TaskDataBaseContext taskDataBaseContext, IJWTRepository jwtRepository, IUserRepository userRepository)
        {
            this.taskDataBaseContext = taskDataBaseContext;
            _jwtRepository = jwtRepository;
            _userRepository = userRepository;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TUser>> login(TUser user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.UName) || string.IsNullOrEmpty(user.UPassword))
                {
                    return BadRequest("Invalid username or password");
                }
                var user1 = await _userRepository.GetUserByName(user.UName);
                var userindata = await _userRepository.GetUserByName(user1.UName);
                var hashedPassword = await _jwtRepository.ValidateHashingPasswordAsync(user.UPassword);

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
