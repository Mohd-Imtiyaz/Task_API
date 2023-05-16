using System.Security.Cryptography;
using System.Text;
using Task_API.DBContext;
using Task_API.Interfaces;
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

        // This function is used to hash password
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
    }
}
