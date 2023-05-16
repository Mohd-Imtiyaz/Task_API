using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DBContext;
using Task_API.Interfaces;

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

        //[HttpPost]
        //[Route("AddUser")]
        //public
    }
}
