using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskDataBaseContext _taskDataBaseContext;

        public UserRepository(TaskDataBaseContext taskDataBaseContext)
        {
            _taskDataBaseContext = taskDataBaseContext;
        }

        // This meathod is used to hash password
        public async Task<TUser> HashingPasswordAsync(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            using var sha256 = SHA256.Create();
            var hashbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashpassword = Convert.ToBase64String(hashbytes);
            return await Task.FromResult(new TUser { UPassword = hashpassword });
        }

        // This meathod is used to get user by ID
        public async Task<TUser> GetUserAccountById(int id)
        {
            var getuser = await _taskDataBaseContext.TUsers.Where(u => u.UId == id).FirstOrDefaultAsync();
            return getuser;
        }

        // This meathod is used to get user By Username
        public async Task<TUser> GetUserByName(string Username)
        {
            var user = await _taskDataBaseContext.TUsers.Where(u => u.UUserName == Username).FirstOrDefaultAsync();
            return user;
        }


        // This meathod is used to add a new user
        public async Task<MUser> AddUserAccount(MUser muser)
        {
            var hashedpassword = await HashingPasswordAsync(muser.UPassword);
            var adduser = new TUser();


            adduser.UName = muser.UName;
            adduser.UUserName = muser.UUserName;
            adduser.UPassword = hashedpassword.UPassword;
            adduser.Roles = muser.Roles;
            adduser.UEmail = muser.UEmail;
            adduser.ActiveStatus = muser.ActiveStatus;

            await _taskDataBaseContext.TUsers.AddAsync(adduser);
            await _taskDataBaseContext.SaveChangesAsync();

            return muser;
        }

        // This meathod is used to update a user by ID
        public async Task<TUser> UpdateUserAccountByID(TUser user)
        {
            var hashingPassword = await HashingPasswordAsync(user.UPassword);

            var upuser = await _taskDataBaseContext.TUsers.Where(u => u.UId == user.UId).FirstOrDefaultAsync();
            upuser.UName = user.UName;
            upuser.UPassword = hashingPassword.UPassword;
            upuser.Roles = user.Roles;
            upuser.UEmail = user.UEmail;
            upuser.ActiveStatus = user.ActiveStatus;

            _taskDataBaseContext.TUsers.Update(upuser);
            return upuser;
        }

        // This meathod is used to delete a user
        public async Task<TUser> DeleteUser(int id)
        {
            var delUser = await _taskDataBaseContext.TUsers.Where(u => u.UId == id).FirstOrDefaultAsync();
            _taskDataBaseContext.TUsers.Remove(delUser);
            _taskDataBaseContext.SaveChangesAsync();
            return delUser;
        }
    }
}

