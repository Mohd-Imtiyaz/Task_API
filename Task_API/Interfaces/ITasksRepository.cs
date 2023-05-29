using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface ITasksRepository
    {
        Task<TUserTask> GetTasksByTitle(string title);

        Task<MPaginationParameters> GetAllTaskByPage(int page, string loggedinUser, float PageResults);

        Task<IEnumerable<TUserTask>> GetAllTasksOfUser(string userName);

        Task<string> AddTaskForUser(MAddingTask mAddingTask, string userName);

        Task<List<TUserTask>> SearchingWithAnyType(string searchQuery, string loggedinUser);

        Task<string> UpdateTask(MUpdatingTask mUpdatingTask, string taskTitle, string loggedInUser);

        Task<string> DeletingTask(string title, string logedinUser);
    }
}
