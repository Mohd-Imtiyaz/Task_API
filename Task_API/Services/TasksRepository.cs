using Microsoft.EntityFrameworkCore;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Services
{
    public class TasksRepository : ITasksRepository
    {
        TaskDataBaseContext _taskDataBaseContext;

        public TasksRepository(TaskDataBaseContext taskDataBaseContext)
        {
            _taskDataBaseContext = taskDataBaseContext;
        }

        public async Task<TUserTask> GetTasksByTitle(string title)
        {
            var retrivedTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();
            return retrivedTask;
        }

        public async Task<IEnumerable<TUserTask>> GetAllTasksOfUser(string userName)
        {
            var retrivedAllTasks = await _taskDataBaseContext.TUserTasks.Where(u => u.TTaskCreater == userName).ToListAsync();
            return retrivedAllTasks;
        }
        public async Task<MAddingTask> AddTaskForUser(MAddingTask mAddingTask, string userName) // may be i need to add a bool field in paramater
        {
            var addUserTask = new TUserTask();

            addUserTask.TTitle = mAddingTask.TTitle;
            addUserTask.TDescription = mAddingTask.TDescription;
            addUserTask.TTaskCreater = userName; // logic to add current user need to be done here
            addUserTask.TStartDate = mAddingTask.TStartDate;
            addUserTask.TEndDate = mAddingTask.TEndDate;
            addUserTask.TFile = mAddingTask.TFile;

            await _taskDataBaseContext.TUserTasks.AddAsync(addUserTask);
            await _taskDataBaseContext.SaveChangesAsync();

            return mAddingTask;
        }
    }
}
