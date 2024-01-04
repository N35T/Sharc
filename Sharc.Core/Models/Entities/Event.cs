using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Sharc.Core.Abstractions.Interfaces;
using Sharc.Core.ExtensionMethods;

namespace Sharc.Core.Models.Entities; 

public class Event : ICalendarFormatter {
    
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

    public string ToICalendar(bool privateEvent) {
        var builder = new StringBuilder($"""
                BEGIN:VEVENT
                UID:{Id}
                DTSTAMP:{Created.ToRFCTimeString()}
                DTSTART:{StartTime.ToRFCTimeString()}
                DTEND:{EndTime.ToRFCTimeString()}
                SUMMARY:{Summary.Replace("\r\n", "\\n").Replace("\n", "\\n")}
                DESCRIPTION:{Description.Replace("\r\n", "\\n").Replace("\n", "\\n")}
                TRANSP:TRANSPARENT
                STATUS:CONFIRMED
                """);
        builder.Append('\n');
        if (RecurrenceRule is not null) {
            builder.Append(RecurrenceRule.ToICalendar(privateEvent));
            builder.Append('\n');
        }

        foreach (var attendee in Attendees) {
            builder.Append(attendee.ToICalendar(privateEvent));
            builder.Append('\n');
        }

        builder.Append("END:VEVENT");
        return builder.ToString();
    }
}