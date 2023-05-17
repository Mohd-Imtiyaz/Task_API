using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface ITasksRepository
    {
        Task<TUserTask> GetTasksByTitle(string title);

        Task<IEnumerable<TUserTask>> GetAllTasksOfUser(string userName);

        Task<MAddingTask> AddTaskForUser(MAddingTask mAddingTask, string userName);
    }
}
