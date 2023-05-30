using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<MPaginationParameters> GetAllTasks(bool isAccending, int page, float PageResults)
        {
            //var allTask = await _taskDataBaseContext.TUserTasks.ToListAsync();

             
            // trial

            var PageCount = Math.Ceiling(_taskDataBaseContext.TUserTasks.Count() / PageResults);
            var task = await _taskDataBaseContext.TUserTasks.Skip((page - 1) * (int)PageResults).Take((int)PageResults).ToListAsync();

            task = isAccending ? task.OrderByDescending(m => m.TTitle).ToList() : task.OrderBy(m => m.TTitle).ToList();


            var response = new MPaginationParameters
            {
                AllTasks = task,
                CurrentPage = page,
                Pages = (int)PageCount
            };
            return response;
            // end trial

            //return allTask;  
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

        public async Task<string> UpdatingTask(MAdminEditUserTask mAdminEditUserTask, string title)
        {
            var updateTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();

            if (updateTask != null)
            {
                updateTask.TTitle = mAdminEditUserTask.TTitle;
                updateTask.TTaskCreater = mAdminEditUserTask.TTaskCreater;
                updateTask.TDescription = mAdminEditUserTask.TDescription;
                updateTask.TStartDate = mAdminEditUserTask.TStartDate;
                updateTask.TEndDate = mAdminEditUserTask.TEndDate;
                updateTask.TFile = mAdminEditUserTask.TFile;

                _taskDataBaseContext.TUserTasks.Update(updateTask);
                _taskDataBaseContext.SaveChangesAsync();

                return "Updated successfully...";
            }
            else
            {
                return "Title not found...";
            }
        }

        public async Task<string> ReAssignTask(string taskTitle, string assigningToUserName)
        {
            var updatingTaskCreator = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == taskTitle).FirstOrDefaultAsync();

            if(updatingTaskCreator != null)
            {
                updatingTaskCreator.TTaskCreater = assigningToUserName;

                _taskDataBaseContext.TUserTasks.Update(updatingTaskCreator);
                _taskDataBaseContext.SaveChangesAsync();

                return "Task Reassigned Successfully...";
            }
            return "Task Not found...";
        }

        public async Task<string> UserStatusUpdate(string userName, string userStatus)
        {
            var updatingUserStatus = await _taskDataBaseContext.TUsers.Where(u => u.UUserName == userName).FirstOrDefaultAsync();

            if(updatingUserStatus != null)
            {
                updatingUserStatus.ActiveStatus = userStatus;

                _taskDataBaseContext.TUsers.Update(updatingUserStatus);
                _taskDataBaseContext.SaveChangesAsync();
                return "Status updated";
            }
            return "User not found...";
        }

        public async Task<string> DeletingTask(string title)
        {
            var taskToDelete = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();

            if (taskToDelete != null)
            {
                _taskDataBaseContext.TUserTasks.Remove(taskToDelete);
                _taskDataBaseContext.SaveChangesAsync();
                return "Task deleted";
            }
            return "Task not found...";
        }
    }
}
