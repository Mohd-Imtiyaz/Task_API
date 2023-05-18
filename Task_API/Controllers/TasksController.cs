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

        public TasksController(TaskDataBaseContext taskDataBaseContext, ITasksRepository tasksRepository, IUserRepository userRepository)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _tasksRepository = tasksRepository;
            _userRepository = userRepository;
        }



        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult> GetAllTasks()
        {
            try
            {
                string loggedinUser = HttpContext.User.FindFirstValue("UserName"); // code to get username who is logged in

                var taskList = await _tasksRepository.GetAllTasksOfUser(loggedinUser);
                if(taskList == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(taskList);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Intrnal Server error...");
            }
        }

        [HttpGet]
        [Route("GetTasksByTitle")]
        public async Task<ActionResult> GetTasksByTitle(string title)
        {
            try
            {
                var taskByTitle = await _tasksRepository.GetTasksByTitle(title);
                if(taskByTitle == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(taskByTitle);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Intrnal Server error...");
            }
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
                {
                    return Conflict("Task already Exists");
                }
                var userAddTask = await _tasksRepository.AddTaskForUser(mAddingTask, loggedinUser);
                return CreatedAtAction(nameof(AddTask), userAddTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error...");
            }
        }


    }
}
