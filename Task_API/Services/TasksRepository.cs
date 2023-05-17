using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.Model;

namespace Task_API.Services
{
    public class TasksRepository : ITasksRepository
    {
        TaskDataBaseContext _taskDataBaseContext;
        private readonly UserRepository _userRepository;

        public TasksRepository(TaskDataBaseContext taskDataBaseContext, UserRepository userRepository)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _userRepository = userRepository;
        }

        //public async Task<TUserTask> GetTasksBy()
    }
}
