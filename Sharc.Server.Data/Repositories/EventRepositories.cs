using Microsoft.EntityFrameworkCore;
using Sharc.Core.Abstractions;
using Sharc.Core.Entities;
using Sharc.Core.Repositories;
using Sharc.Server.Data.Persistence;

namespace Sharc.Server.Data.Repositories; 

internal class EventRepositories : IEventRepository {

    private readonly ApplicationDbContext _dbContext;
    
    public EventRepositories(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<Result<Event>> AddEventAsync(Event eventModel) {
        _dbContext.Events.Add(eventModel);
        return await _dbContext.SaveChangesAsync() > 0 ? eventModel : new Exception();
    }

    public Task<Result> AddUsersToEventAsync(Guid eventId, params User[] users) {
        return AddUsersToEventAsync(eventId, users.Select(e => e.Id).ToArray());
    }

    public async Task<Result> AddUsersToEventAsync(Guid eventId, params Guid[] users) {
        _dbContext.EventUsers.AddRange(
            users.Select(e => new EventUsers {
                EventId = eventId,
                UserId = e
        }));

        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task<List<Event>> GetUserEventsAsync(Guid userId) {
        return _dbContext
            .EventUsers
            .Include(e => e.Event)
            .Where(e => e.UserId == userId)
            .Select(e => e.Event)
            .Include(e => e.Attendees)
            .ToListAsync();
    }

    public async Task<Event?> GetEventAsync(Guid eventId) {
        return await _dbContext
            .Events
            .FindAsync(eventId);
    }

    public async Task<Result<Event>> UpdateEventAsync(Event eventModel) {
        _dbContext.Events.Update(eventModel);

        return await _dbContext.SaveChangesAsync() > 0 ? eventModel : new Exception();
    }

    public async Task<Result> DeleteEventAsync(Guid eventId) {
        var eventModel = await _dbContext.Events.FindAsync(eventId);
        if (eventModel is null) {
            return false;
        }

        _dbContext.Remove(eventModel);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Result> RemoveUserFromEventAsync(Guid eventId, Guid userId) {
        var eventUser = await _dbContext.EventUsers.FindAsync(userId, eventId);

        if (eventUser is null) {
            return false;
        }

        _dbContext.Remove(eventUser);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task<Result> RemoveUserFromEventAsync(Guid eventId, User user) {
        return RemoveUserFromEventAsync(eventId, user.Id);
    }
}