using Sharc.Core.Abstractions;
using Sharc.Core.Models.Entities;

namespace Sharc.Server.Core.Repositories; 

public interface IUserRepository {

    Task<List<User>> GetAllUsersAsync();

    Task<Result<User>> AddNewUserAsync(User user);

    Task<Result> UpdateCachedUsernameAsync(Guid userId, string username);

    Task<User?> GetUserAsync(Guid userId);
}