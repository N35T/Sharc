using System.ComponentModel.DataAnnotations.Schema;
using Sharc.Core.ExtensionMethods;

namespace Sharc.Core.Entities; 

public class Event {
    
    public Guid Id { get; set; }
    // CN
    public string Name { get; set; }
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
    public User[] Attendees { get; set; }


    [NotMapped]
    public TimeSpan Duration {
        get => EndTime.Subtract(StartTime);
        set => EndTime = StartTime.Add(value);
    }

    public override string ToString() {
        var s = $"""
                BEGIN:VEVENT
                UID:{Created.ToRFCTimeString()}-{Id}
                CN:{Name}
                DTSTAMP:{Created.ToRFCTimeString()}
                DTSTART:{StartTime.ToRFCTimeString()}
                DTEND:{EndTime.ToRFCTimeString()}
                SUMMARY:{Summary}
                DESCRIPTION:{Description}
                """;
        if (RecurrenceRule is not null) {
            s += RecurrenceRule;
        }

        return s + "END:VEVENT";
    }
}