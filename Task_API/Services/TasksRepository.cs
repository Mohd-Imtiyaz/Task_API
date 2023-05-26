using Microsoft.EntityFrameworkCore;
using System.Linq;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Services
{
    public class TasksRepository : ITasksRepository
    {
        TaskDataBaseContext _taskDataBaseContext;
        private readonly IUserRepository _userRepository;

        public TasksRepository(TaskDataBaseContext taskDataBaseContext, IUserRepository userRepository)
        {
            _taskDataBaseContext = taskDataBaseContext;
            _userRepository = userRepository;
        }



        public async Task<TUserTask> GetTasksByTitle(string title)
        {
            var retrivedTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title).FirstOrDefaultAsync();
            return retrivedTask;
        }


        
        public async Task<List<TUserTask>> SearchingWithAnyType(string searchQuery, string loggedinUser)
        {
            var UserTasks = await _taskDataBaseContext.TUserTasks.ToListAsync();

            var matchingTasks = UserTasks
                .Where(m => m.TTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) && m.TTaskCreater == loggedinUser)
                .ToList();

            return matchingTasks;
        }

        public async Task<IEnumerable<TUserTask>> GetAllTasksOfUser(string loggedinUser)
        {
            var retrivedAllTasks = await _taskDataBaseContext.TUserTasks.Where(u => u.TTaskCreater == loggedinUser).ToListAsync();

            return retrivedAllTasks;
        }



        
        // start
        public async Task<MPaginationParameters> GetAllTaskByPage(int page, string loggedinUser,float PageResults)
        {
            //var PageResults = x; 
            var PageCount = Math.Ceiling(_taskDataBaseContext.TUserTasks.Where(u => u.TTaskCreater == loggedinUser).Count() / PageResults);

            var task = await _taskDataBaseContext.TUserTasks.Skip((page - 1) * (int)PageResults).Take((int)PageResults).Where(u => u.TTaskCreater == loggedinUser).ToListAsync();

            var response = new MPaginationParameters
            {
                AllTasks = task,
                CurrentPage = page,
                Pages = (int)PageCount
            };

            return response;
        }

        // end
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

        public async Task<MUpdatingTask> UpdateTask(MUpdatingTask mUpdatingTask, string taskTitle, string loggedInUser)
        {
            var gettingUserTask = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == taskTitle && u.TTaskCreater == loggedInUser).FirstOrDefaultAsync();

            if (gettingUserTask != null)
            {
                gettingUserTask.TTitle = mUpdatingTask.TTitle;
                gettingUserTask.TDescription = mUpdatingTask.TDescription;
                gettingUserTask.TStartDate = mUpdatingTask.TStartDate;
                gettingUserTask.TEndDate = mUpdatingTask.TEndDate;
                gettingUserTask.TFile = mUpdatingTask.TFile;

                _taskDataBaseContext.TUserTasks.Update(gettingUserTask);
                _taskDataBaseContext.SaveChangesAsync();

                return mUpdatingTask;
            }
            else
            {
                mUpdatingTask.TTitle = "Title Not Found";
                return mUpdatingTask;
            }
        }

        public async Task<TUserTask> DeletingTask(string title, string logedinUser)
        {

            var loggedinUserTasks = await _taskDataBaseContext.TUserTasks.Where(u => u.TTitle == title && u.TTaskCreater == logedinUser).FirstOrDefaultAsync();

            _taskDataBaseContext.TUserTasks.Remove(loggedinUserTasks);
            _taskDataBaseContext.SaveChangesAsync();
            return loggedinUserTasks;
        }
    }
}
