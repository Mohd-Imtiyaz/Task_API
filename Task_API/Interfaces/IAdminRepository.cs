using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<TUser>> SearchingUserWithUName(string username);

        Task<IEnumerable<TUserTask>> GetAllTasks(bool isAccending);

        Task<IEnumerable<TUserTask>> GetAllTaskFromUsername(string username);

        Task<List<TUserTask>> SearchAnyTask(string searchQuery);

        Task<MUpdatingTask> UpdatingTask(MUpdatingTask mUpdatingTask);

        Task<TUserTask> ReAssignTask(string taskTitle, string assigningToUserName);

        Task<TUserTask> DeletingTask(string title);
    }
}
