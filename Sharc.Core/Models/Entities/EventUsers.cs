using System.Text;
using Sharc.Core.Abstractions.Interfaces;
using Sharc.Core.Exceptions;

namespace Sharc.Core.Models.Entities; 

public class EventUsers : ICalendarFormatter {
    
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    
    public Event? Event { get; set; }
    public User? User { get; set; }
    
    public string ToICalendar(bool privateEvent) {
        if (User is null) {
            throw new ICalendarException("User is null");
        }
        var builder = new StringBuilder();
        builder.Append("\nATTENDEE;CN=");
        builder.Append(User.CachedUsername);
        builder.Append(":mailto:");
        builder.Append(User.Email);
        
        return builder.ToString();
    }
}