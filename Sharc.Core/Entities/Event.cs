using System.ComponentModel.DataAnnotations.Schema;

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

    // ATTENDEE
    public User[] Attendees { get; set; }


    [NotMapped]
    public TimeSpan Duration {
        get => EndTime.Subtract(StartTime);
        set => EndTime = StartTime.Add(value);
    }

    public override string ToString() {
        return $"""
                BEGIN:VEVENT
                UID:{DateTimeToRFCTime(Created)}-{Id}
                CN:{Name}
                DTSTAMP:{DateTimeToRFCTime(Created)}
                DTSTART:{DateTimeToRFCTime(StartTime)}
                DTEND:{DateTimeToRFCTime(EndTime)}
                SUMMARY:{Summary}
                DESCRIPTION:{Description}
                END:VEVENT
                """;
    }

    private static string DateTimeToRFCTime(DateTime time) {
        return time.ToUniversalTime().ToString("yyyyMMddTHHmmss");
    }
}