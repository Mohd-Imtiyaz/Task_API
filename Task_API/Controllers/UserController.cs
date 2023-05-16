using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        TaskDataBaseContext _taskDataBaseContext;
        private readonly IUserRepository _userRepository;

        public UserController(TaskDataBaseContext taskDataBaseContext, IUserRepository userRepository)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<MUser>> AddUser(MUser muser)
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
                return StatusCode(500, "Internal Server Error...");
            }
        }


        [HttpGet("GetValue")]
        public async Task<ActionResult> getvalue(string username)
        {
            try
            {
                var use = await _userRepository.GetUserByName(username);
                if (use != null)
                {
                    return Ok(use);
                }
                else
                {
                    return StatusCode(404, "There is no user with this Username...");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error...");
            }
        }


        [HttpPut("UpdateUser")]
        public async Task<ActionResult> updateuser(TUser user)
        {
            try
            {
                var updatedUser = await _userRepository.UpdateUserAccountByID(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error...");
            }
        }


        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> delUser(int id)
        {
            try
            {
                var dellUSer = await _userRepository.DeleteUser(id);
                return Ok(dellUSer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error...");
            }
        }


    }
}






