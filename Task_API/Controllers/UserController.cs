using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
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
        


        /// <summary>
        /// Adding the User
        /// </summary>
        /// <param name="muser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<MUser>> AddUser(MUser muser)
        {
            //var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex re = new Regex(strRegex);
                    if (re.IsMatch(muser.UEmail))
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
                    else
                    {
                        return StatusCode(400, "enter a valid Email");
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



        /// <summary>
        /// Getting the User with username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

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



        /// <summary>
        /// Updating the User details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Deleting the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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






