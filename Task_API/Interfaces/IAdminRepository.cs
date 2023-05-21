using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<TUserTask>> GetAllTaskFromUsername(string username);

        Task<IEnumerable<TUserTask>> GetAllTasks(bool isAccending);

        Task<List<TUserTask>> SearchAnyTask(string searchQuery);

        Task<MAdminEditUserTask> UpdatingTask(MAdminEditUserTask mAdminEditUserTask, string title);

        Task<TUserTask> ReAssignTask(string taskTitle, string assigningToUserName);

        Task<TUserTask> DeletingTask(string title);
    }
}
