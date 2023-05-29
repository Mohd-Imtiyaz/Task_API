using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IUserRepository
    {
        Task<TUser> HashingPasswordAsync(string password);

        Task<TUser> GetUserAccountById(int id);

        Task<TUser> GetUserByName(string Username);

        Task<string> AddUserAccount(MUser muser);

        Task<string> UpdateUserAccountByID(TUser user);

        Task<string> DeleteUser(int id);

        Task<string> UserIsActiveOrNot(string userName);
    }
}
