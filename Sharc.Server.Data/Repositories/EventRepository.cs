using Microsoft.EntityFrameworkCore;
using Sharc.Core.Abstractions;
using Sharc.Core.Exceptions;
using Sharc.Core.Models.Entities;
using Sharc.Server.Core.Repositories;
using Sharc.Server.Data.Persistence;

namespace Sharc.Server.Data.Repositories; 

internal class EventRepository : IEventRepository {

    private readonly ApplicationDbContext _dbContext;
    
    public EventRepository(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<Result<Event>> AddEventAsync(Event eventModel) {
        _dbContext.Events.Add(eventModel);
        return await _dbContext.SaveChangesAsync() > 0 ? eventModel : new EventOperationException("Failed adding the event");
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

        return await _dbContext.SaveChangesAsync() > 0 ? true : new EventOperationException("Failed adding the users to the event");
    }

    public Task<List<Event>> GetUserEventsAsync(Guid userId) {
        return _dbContext
            .EventUsers
            .Include(e => e.Event)
            .Where(e => e.Event!.IsPublic || e.UserId == userId)
            .Select(e => e.Event!)
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

        return await _dbContext.SaveChangesAsync() > 0 ? eventModel : new EventOperationException("Failed updating the event");
    }

    public async Task<Result> DeleteEventAsync(Guid eventId) {
        var eventModel = await _dbContext.Events.FindAsync(eventId);
        if (eventModel is null) {
            return new EventOperationException("Could not find the event with id " + eventId);
        }

        _dbContext.Remove(eventModel);
        return await _dbContext.SaveChangesAsync() > 0 ? true : new EventOperationException("Failed deleting the event");
    }

    public async Task<Result> RemoveUserFromEventAsync(Guid eventId, Guid userId) {
        var eventUser = await _dbContext.EventUsers.FindAsync(userId, eventId);

        if (eventUser is null) {
            return new UserNotFoundException("User with id " + userId + " is not part of event with id " + eventId);
        }

        _dbContext.Remove(eventUser);
        return await _dbContext.SaveChangesAsync() > 0 ? true : new EventOperationException("Failed removing user from event");
    }

    public Task<Result> RemoveUserFromEventAsync(Guid eventId, User user) {
        return RemoveUserFromEventAsync(eventId, user.Id);
    }
}