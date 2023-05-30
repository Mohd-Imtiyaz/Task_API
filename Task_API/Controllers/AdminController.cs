using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
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

        /// <summary>
        /// Gets all Tasks created is selected order
        /// </summary>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        // This function allows you to 
        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult> GetAllTasks(bool isDesending, int page, float PageResults)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {

                try
                {
                    var allTasks = await _adminRepository.GetAllTasks(isDesending, page, PageResults);
                    return StatusCode(200, allTasks);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");

        }


        /// <summary>
        /// Getting all tasks of the perticular user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");

        }


        /// <summary>
        /// Retriving the Task by providing Task Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
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
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");
        }



        /// <summary>
        /// Updating any Task by providing the Title
        /// </summary>
        /// <param name="mAdminEditUserTask"></param>
        /// <param name="titleToBeUpdated"></param>
        /// <returns></returns>
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
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");
        }



        /// <summary>
        /// Reassigning the Task to any User
        /// </summary>
        /// <param name="titleToAssign"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
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
                    return StatusCode(202, reassingning);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstrationn...");
        }



        /// <summary>
        /// Updating the user status
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="setStatusTo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatingUserStatus")]
        public async Task<ActionResult> UserStatusUpdate(string userName, bool setStatusTo)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    if(setStatusTo == true)
                    {
                        var userStatus = "Active";
                        var userNewStatus = await _adminRepository.UserStatusUpdate(userName, userStatus);
                    }
                    else
                    {
                        var userStatus = "Disabled";
                        var userNewStatus = await _adminRepository.UserStatusUpdate(userName, userStatus);
                    }
                    return StatusCode(200, "User status updated Sucessfully...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, "Internal Server Error...");
                }
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");
        }



        /// <summary>
        /// Deleting a task by providing the title
        /// </summary>
        /// <param name="taskTitle"></param>
        /// <returns></returns>
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
            return StatusCode(404, "Your status is Inactive please contact Adminstration...");
        }

    }
}
