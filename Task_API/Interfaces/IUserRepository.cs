using Task_API.ManualClasses;
using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IUserRepository
    {
        Task<TUser> HashingPasswordAsync(string password);

        Task<TUser> GetUserAccountById(int id);

        Task<TUser> GetUserByName(string Username);

        Task<MUser> AddUserAccount(MUser muser);

        Task<TUser> UpdateUserAccountByID(TUser user);

        Task<TUser> DeleteUser(int id);
    }
}
