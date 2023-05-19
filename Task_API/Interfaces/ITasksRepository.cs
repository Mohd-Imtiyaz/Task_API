using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface ITasksRepository
    {
        Task<TUserTask> GetTasksByTitle(string title);

        Task<MPaginationParameters> GetAllTaskByPage(int page, string userName);
        Task<IEnumerable<TUserTask>> GetAllTasksOfUser(string userName);

        Task<MAddingTask> AddTaskForUser(MAddingTask mAddingTask, string userName);

        Task<List<TUserTask>> SearchingWithAnyType(string searchQuery, string loggedinUser, bool isAccending);

        Task<MUpdatingTask> UpdateTask(MUpdatingTask mUpdatingTask, string taskTitle, string loggedInUser);

        Task<TUserTask> DeletingTask(string title, string logedinUser);
    }
}
