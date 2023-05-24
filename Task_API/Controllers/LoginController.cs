using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTRepository _jwtRepository;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IJWTRepository jwtRepository, IUserRepository userRepository, ILogger<LoginController> logger)
        {
            _jwtRepository = jwtRepository;
            _userRepository = userRepository;
            _logger = logger;
        }



        [HttpPost("Login")]
        public async Task<ActionResult<MLogin>> Login(MLogin mLogin)
        {
            try
            {
                var user = await _userRepository.GetUserByName(mLogin.UUserName);

                if (mLogin == null || string.IsNullOrEmpty(user.UUserName) || string.IsNullOrEmpty(user.UPassword))
                {
                    if(user.ActiveStatus != "Active")
                    {
                        return BadRequest("You are inactive user...");
                    }
                    return BadRequest("Invalid username or password");
                }
                var user1 = await _userRepository.GetUserByName(user.UUserName);
                var userindata = await _userRepository.GetUserByName(user1.UUserName);
                var hashedPassword = await _jwtRepository.ValidateHashingPasswordAsync(mLogin.UPassword);

                if (userindata.UPassword != hashedPassword.UPassword)
                {
                    return BadRequest("Invalid password");
                }
                var Token = await _jwtRepository.TokenGenerate(user1);
                return StatusCode(200, Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Servar Error"+ex);
            }

        }
    }
}
