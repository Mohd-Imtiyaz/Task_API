using Azure;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        TaskDataBaseContext _taskDataBaseContext;
        private readonly ITasksRepository _tasksRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskDataBaseContext taskDataBaseContext, ITasksRepository tasksRepository, IUserRepository userRepository, ILogger<TasksController> logger)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _tasksRepository = tasksRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        
        /// <summary>
        /// Getting all Tasks of loggedin User
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult<MPaginationParameters>> GetAllTasks(int page, float recordsPerPage)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {

                    //string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is logged in

                    var taskList = await _tasksRepository.GetAllTaskByPage(page, loggedinUser, recordsPerPage);

                    if (taskList == null)
                    {
                        return StatusCode(404, "Data not found");
                    }
                    else
                    {
                        return StatusCode(200, taskList);
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
        /// Searching Task with help of Title (also excepts similar string)
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchTask(string searchQuery)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                //string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
                var searchedTask = await _tasksRepository.SearchingWithAnyType(searchQuery, loggedinUser);
                return StatusCode(200, searchedTask);
            }
            return StatusCode(404, "Your status is Inactive please contact Adminstratio...");
        }


        /// <summary>
        /// Adding Task which will be assinged to the Loggedin user
        /// </summary>
        /// <param name="mAddingTask"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTask")]
        public async Task<ActionResult<MAddingTask>> AddTask(MAddingTask mAddingTask)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {

                    //string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 

                    var existingTask = await _tasksRepository.GetTasksByTitle(mAddingTask.TTitle);
                    if (existingTask != null)
                        if (existingTask != null)
                        {
                            return Conflict("Task already Exists");
                        }
                    var userAddTask = await _tasksRepository.AddTaskForUser(mAddingTask, loggedinUser);
                    return CreatedAtAction(nameof(AddTask), userAddTask);
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
        /// Updating Task of the user 
        /// </summary>
        /// <param name="mUpdatingTask"></param>
        /// <param name="Title_Name"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTask")]
        public async Task<ActionResult<MUpdatingTask>> UpdateTask(MUpdatingTask mUpdatingTask, string Title_Name)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var updatingTask = await _tasksRepository.UpdateTask(mUpdatingTask, Title_Name, loggedinUser);
                    return StatusCode(200, "Data Updated Sucessfully");
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
        /// Deleting the tasks of the user
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<ActionResult> DeleteTask(string title)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 
            var userIsValidOrNo = await _userRepository.UserIsActiveOrNot(loggedinUser);
            if (userIsValidOrNo == "Active")
            {
                try
                {
                    var delTask = _tasksRepository.DeletingTask(title, loggedinUser);
                    return StatusCode(200, "Task deleted succesfully");
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
