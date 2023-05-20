using Microsoft.EntityFrameworkCore;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TaskDataBaseContext _taskDataBaseContext;

        public AdminRepository(TaskDataBaseContext taskDataBaseContext)
        {
            _taskDataBaseContext = taskDataBaseContext;
        }

        public async Task<IEnumerable<TUserTask>> GetAllTasks(bool isAccending)
        {
            var allTask = await _taskDataBaseContext.TUserTasks.ToListAsync();

            allTask = isAccending ? allTask.OrderByDescending(m => m.TTitle).ToList() : allTask.OrderBy(m => m.TTitle).ToList();

            return allTask;
        }


        public async Task<List<TUserTask>> GetAllTaskFromUsername(string username)
        {
            var UserTasks = await _taskDataBaseContext.TUserTasks.ToListAsync();

            var matchingTasks = UserTasks.Where(m => m.TTaskCreater.Contains(username, StringComparison.OrdinalIgnoreCase)).ToList();


            return matchingTasks;
        }

        

        public async Task<List<TUserTask>> SearchAnyTask(string searchQuery)
        {
            var UserTasks = await _taskDataBaseContext.TUserTasks.ToListAsync();

            var matchingTasks = UserTasks.Where(m => m.TTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            return matchingTasks;
        }

        public async Task<TUserTask> UpdatingTask(TUserTask userTask, string title)
        {
            var updateTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();

            if (updateTask != null)
            {
                updateTask.TTitle = userTask.TTitle;
                updateTask.TTaskCreater = userTask.TTaskCreater;
                updateTask.TDescription = userTask.TDescription;
                updateTask.TStartDate = userTask.TStartDate;
                updateTask.TEndDate = userTask.TEndDate;
                updateTask.TFile = userTask.TFile;

                _taskDataBaseContext.TUserTasks.Update(updateTask);
                _taskDataBaseContext.SaveChangesAsync();

                return userTask;
            }
            else
            {
                userTask.TTitle = "Title Not Found";
                return userTask;
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
