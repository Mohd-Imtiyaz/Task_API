using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        TaskDataBaseContext _taskDataBaseContext;
        private readonly ITasksRepository _tasksRepository;

        public TasksController(TaskDataBaseContext taskDataBaseContext, ITasksRepository tasksRepository)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _tasksRepository = tasksRepository;
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult> GetAllTasks()
        {
            try
            {
                var taskList = await _tasksRepository.GetAllTasksOfUser(Username);
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


    }
}
