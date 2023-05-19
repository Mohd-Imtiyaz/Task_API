using Microsoft.EntityFrameworkCore;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Services
{
    public class AdminRepository : IAdminRepository
    {
        TaskDataBaseContext _taskDataBaseContext;

        public AdminRepository(TaskDataBaseContext taskDataBaseContext)
        {
            _taskDataBaseContext = taskDataBaseContext;
        }

        public async Task<List<TUser>> SearchingUserWithUName(string username)
        {
            var retrivedUser = await _taskDataBaseContext.TUsers.ToListAsync();

            var retrivedUserList = retrivedUser.Where(u => u.UUserName.Contains(username, StringComparison.OrdinalIgnoreCase)).ToList();

            return retrivedUserList;

        }

        public async Task<IEnumerable<TUserTask>> GetAllTasks(bool isAccending)
        {
            var allTask = await _taskDataBaseContext.TUserTasks.ToListAsync();

            allTask = isAccending ? allTask.OrderByDescending(m => m.TTitle).ToList() : allTask.OrderBy(m => m.TTitle).ToList();

            return allTask;
        }

        public async Task<IEnumerable<TUserTask>> GetAllTaskFromUsername(string username)
        {
            var taskByUsername = await _taskDataBaseContext.TUserTasks.Where(m => m.TTaskCreater.Contains(username, StringComparison.OrdinalIgnoreCase)).ToListAsync();

            return taskByUsername;
        }

        public async Task<List<TUserTask>> SearchAnyTask(string searchQuery)
        {
            var UserTasks = await _taskDataBaseContext.TUserTasks.ToListAsync();

            var matchingTasks = UserTasks.Where(m => m.TTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();


            return matchingTasks;
        }

        public async Task<MUpdatingTask> UpdatingTask(MUpdatingTask mUpdatingTask)
        {
            var updateTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == mUpdatingTask.TTitle).FirstOrDefaultAsync();

            if (updateTask != null)
            {
                updateTask.TTitle = mUpdatingTask.TTitle;
                updateTask.TDescription = mUpdatingTask.TDescription;
                updateTask.TStartDate = mUpdatingTask.TStartDate;
                updateTask.TEndDate = mUpdatingTask.TEndDate;
                updateTask.TFile = mUpdatingTask.TFile;

                _taskDataBaseContext.TUserTasks.Update(updateTask);
                _taskDataBaseContext.SaveChangesAsync();

                return mUpdatingTask;
            }
            else
            {
                mUpdatingTask.TTitle = "Title Not Found";
                return mUpdatingTask;
            }
        }

        public async Task<TUserTask> ReAssignTask(string taskTitle, string assigningToUserName)
        {
            var updatingTaskCreator = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == taskTitle).FirstOrDefaultAsync();

            updatingTaskCreator.TTaskCreater = assigningToUserName;

            _taskDataBaseContext.TUserTasks.Update(updatingTaskCreator);
            _taskDataBaseContext.SaveChangesAsync();

            return updatingTaskCreator;
        }

        public async Task<TUserTask> DeletingTask(string title)
        {
            var taskToDelete = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();

            _taskDataBaseContext.TUserTasks.Remove(taskToDelete);
            _taskDataBaseContext.SaveChangesAsync();
            return taskToDelete;
        }
    }
}
