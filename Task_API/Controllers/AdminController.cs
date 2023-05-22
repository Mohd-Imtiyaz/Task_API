using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        TaskDataBaseContext TaskDataBaseContext;
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(TaskDataBaseContext taskDataBaseContext, IAdminRepository adminRepository, ITasksRepository tasksRepository, ILogger<AdminController> logger, IUserRepository userRepository)
        {
            TaskDataBaseContext = taskDataBaseContext;
            _adminRepository = adminRepository;
            _tasksRepository = tasksRepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult> GetAllTasks(bool isAccending)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Activea")
            {

                try
                {
                    var allTasks = await _adminRepository.GetAllTasks(isAccending);
                    return StatusCode(200, allTasks);
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
        [Route("GetAllTaskOfUser")]
        public async Task<ActionResult> GetAllTaskOfUser(string userName)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var taskList = await _adminRepository.GetAllTaskFromUsername(userName);
                    if (taskList == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return StatusCode(200, taskList);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Intrnal Server error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");

        }

        [HttpGet]
        [Route("SearchingFromTitle")]
        public async Task<ActionResult> SearchingFromTitle(string title)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var task = await _adminRepository.SearchAnyTask(title);
                    return StatusCode(200, task);
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
        [Route("UpdatingTask")]
        public async Task<ActionResult<MAdminEditUserTask>> UpdatingTask(MAdminEditUserTask mAdminEditUserTask, string titleToBeUpdated)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var updatingTask = await _adminRepository.UpdatingTask(mAdminEditUserTask, titleToBeUpdated);
                    return StatusCode(202, "Task Updated Successfully...");
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
        [Route("ReAssignTaskToUser")]
        public async Task<ActionResult> ReAssignTaskToUser(string titleToAssign, string newUser)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var reassingning = await _adminRepository.ReAssignTask(titleToAssign, newUser);
                    return StatusCode(202, "Task Re Assigned Successfully...");
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
        [Route("DeleteTask")]
        public async Task<ActionResult> DeleteTask(string taskTitle)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var delTask = await _adminRepository.DeletingTask(taskTitle);
                    return StatusCode(200, "Task Deleted Sucessfully...");
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
