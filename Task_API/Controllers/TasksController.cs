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
    [Authorize]

    [Route("api/[controller]")]
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

        

        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult<MPaginationParameters>> GetAllTasks(int page)
        {
            try
            {

                string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is logged in

                var taskList = await _tasksRepository.GetAllTaskByPage(page, loggedinUser);

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

        [HttpGet("search")]
        public async Task<IActionResult> SearchTask(string searchQuery, bool assendingOrder)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin
            var searchedTask = await _tasksRepository.SearchingWithAnyType(searchQuery, loggedinUser, assendingOrder);

            return StatusCode(200, searchedTask);
        }


        [HttpPost]
        [Route("AddTask")]
        public async Task<ActionResult<MAddingTask>> AddTask(MAddingTask mAddingTask)
        {
            try
            {

                string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 

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

        [HttpPut]
        [Route("UpdateTask")]
        public async Task<ActionResult<MUpdatingTask>> UpdateTask(MUpdatingTask mUpdatingTask, string Title_Name)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 

            try
            {
                var updatingTask = await _tasksRepository.UpdateTask(mUpdatingTask, Title_Name, loggedinUser);
                return StatusCode(404, "Data not found...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<ActionResult> DeleteTask(string title)
        {
            string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is loggedin 

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

    }
}
