using Microsoft.EntityFrameworkCore;
using Sharc.Core.Abstractions;
using Sharc.Core.Exceptions;
using Sharc.Core.Models.Entities;
using Sharc.Server.Core.Repositories;
using Sharc.Server.Data.Persistence;

namespace Sharc.Server.Data.Repositories; 

internal class UserRepository : IUserRepository {

    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public Task<List<User>> GetAllUsersAsync() {
        return _dbContext.Users.ToListAsync();
    }

    public async Task<Result<User>> AddNewUserAsync(User user) {
        await _dbContext.Users.AddAsync(user);

        return await _dbContext.SaveChangesAsync() > 0 ? user : new UserOperationException("Failed adding new user");
    }

    public async Task<Result> UpdateCachedUsernameAsync(Guid userId, string username) {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user is null) {
            return new UserNotFoundException("Could not find user with id " + userId);
        }
        user.CachedUsername = username;
        _dbContext.Users.Update(user);
        return await _dbContext.SaveChangesAsync() > 0 ? true : new UserOperationException("Failed updating the username");
    }

    public async Task<User?> GetUserAsync(Guid userId) {
        return await _dbContext.Users.FindAsync(userId);
    }
}