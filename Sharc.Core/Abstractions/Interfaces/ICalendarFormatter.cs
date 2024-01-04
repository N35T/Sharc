namespace Sharc.Core.Abstractions.Interfaces; 

public interface ICalendarFormatter {

    string ToICalendar(bool privateEvent);
}