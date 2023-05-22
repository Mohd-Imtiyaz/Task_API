using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Controllers
{
    [Authorize]
    [Route("api/Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {

        TaskDataBaseContext _taskDataBaseContext;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(TaskDataBaseContext taskDataBaseContext, IUserRepository userRepository, ILogger<UserController> logger)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<MUser>> AddUser(MUser muser)
        {
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var existingUser = await _userRepository.GetUserByName(muser.UUserName);
                    if (existingUser != null)
                    {
                        return Conflict("Username already exixts...");
                    }
                    var addingUser = await _userRepository.AddUserAccount(muser);
                    await _taskDataBaseContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(AddUser), addingUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");

        }



        [HttpGet]
        [Route("GetValue")]
        [Authorize]
        public async Task<ActionResult> getvalue(string username)
        {
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var use = await _userRepository.GetUserByName(username);
                    if (use != null)
                    {
                        return StatusCode(200, use);
                    }
                    else
                    {
                        return StatusCode(404, "There is no user with this Username...");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");

        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult> updateuser(TUser user)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var updatedUser = await _userRepository.UpdateUserAccountByID(user);
                    return StatusCode(200, updatedUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");

        }


        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult> delUser(int id)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var dellUSer = await _userRepository.DeleteUser(id);
                    return StatusCode(200, "User Deleted Succesfully...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");

        }


    }
}






