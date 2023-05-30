using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<TUserTask>> GetAllTaskFromUsername(string username);

        Task<MPaginationParameters> GetAllTasks(bool isAccending, int page, float PageResults);

        Task<List<TUserTask>> SearchAnyTask(string searchQuery);

        Task<string> UpdatingTask(MAdminEditUserTask mAdminEditUserTask, string title);

        Task<string> ReAssignTask(string taskTitle, string assigningToUserName);

        Task<string> UserStatusUpdate(string userName, string userStatus);

        Task<string> DeletingTask(string title);
    }
}
