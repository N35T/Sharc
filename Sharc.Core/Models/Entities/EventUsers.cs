namespace Sharc.Core.Models.Entities; 

public class EventUsers {
    
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    
    public Event Event { get; set; }
    public User User { get; set; }
}