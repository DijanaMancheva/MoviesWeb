using System.Security.Claims;
using MoviesAPI.Models;

namespace MoviesAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserAsync();
        Task<User> GetUserAsync(int id);
        //Task<UpdateUser> GetUserForUpdateAsync(int id);
        Task<int> CreateUserAsync(CreateUser newUser);
        Task<int> UpdateUserAsync(int id, User newUser);
        Task<int> DeleteUserAsync(int id);
        IEnumerable<Claim> GetUserClaimsAsync();
        Task<int> UpdatePasswordAsync(int id, User newUser);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserForUpdateAsync(int id);
        Task<long> GetDashInfoAsync();
        //Task<User> GetUserByPasswordAsync(int id);
    }
}
