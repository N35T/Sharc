using Sharc.Core.Abstractions;
using Sharc.Core.Models.Entities;

namespace Sharc.Core.Repositories; 

public interface IEventRepository {
    
    Task<Result<Event>> AddEventAsync(Event eventModel);

    Task<Result> AddUsersToEventAsync(Guid eventId, params User[] users);
    Task<Result> AddUsersToEventAsync(Guid eventId, params Guid[] users);

    Task<List<Event>> GetUserEventsAsync(Guid userId);

    Task<Event?> GetEventAsync(Guid eventId);

    Task<Result<Event>> UpdateEventAsync(Event eventModel);

    Task<Result> DeleteEventAsync(Guid eventId);

    Task<Result> RemoveUserFromEventAsync(Guid eventId, Guid userId);
    Task<Result> RemoveUserFromEventAsync(Guid eventId, User user);

}