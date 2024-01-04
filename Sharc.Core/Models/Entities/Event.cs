using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Sharc.Core.ExtensionMethods;

namespace Sharc.Core.Models.Entities; 

public class Event {
    
    public Guid Id { get; set; }
    // DESCRIPTION
    public string Description { get; set; }
    // SUMMARY
    public string Summary { get; set; }
    // DTSTAMP
    public DateTime Created { get; set; }
    // DTEND
    public DateTime EndTime { get; set; }
    // DTSTART
    public DateTime StartTime { get; set; }
    
    // RRULE
    public RecurrenceRule? RecurrenceRule { get; set; }

    // ATTENDEE
    public EventUsers[] Attendees { get; set; }

    public bool IsPublic { get; set; } = false;


    [NotMapped]
    public TimeSpan Duration {
        get => EndTime.Subtract(StartTime);
        set => EndTime = StartTime.Add(value);
    }

    public override string ToString() {
        var builder = new StringBuilder($"""
                BEGIN:VEVENT
                UID:{Created.ToRFCTimeString()}-{Id}
                DTSTAMP:{Created.ToRFCTimeString()}
                DTSTART:{StartTime.ToRFCTimeString()}
                DTEND:{EndTime.ToRFCTimeString()}
                SUMMARY:{Summary.Replace("\r\n", "\\n").Replace("\n", "\\n")}
                DESCRIPTION:{Description.Replace("\r\n", "\\n").Replace("\n", "\\n")}
                TRANSP:TRANSPARENT
                STATUS:CONFIRMED
                """);
        if (RecurrenceRule is not null) {
            builder.Append('\n');
            builder.Append(RecurrenceRule);
            builder.Append('\n');
        }

        foreach (var attendee in Attendees) {
            builder.Append("\nATTENDEE;CN=");
            builder.Append(attendee.User.CachedUsername);
            builder.Append(":mailto:");
            builder.Append(attendee.User.Email);
            builder.Append('\n');
        }

        builder.Append("END:VEVENT");
        return builder.ToString();
    }
}