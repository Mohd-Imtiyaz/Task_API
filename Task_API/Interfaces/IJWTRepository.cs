using Task_API.Model;

namespace Task_API.Interfaces
{
    public interface IJWTRepository
    {
        Task<string> TokenGenerate(TUser user);

        bool TokenValid(string token);

        void RemoveToken(string token);

        Task<TUser> ValidateHashingPasswordAsync(string password);
    }
}
