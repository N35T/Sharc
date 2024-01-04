using System.Runtime.Serialization;

namespace Sharc.Core.Exceptions; 

public class ICalendarException : Exception {
    public ICalendarException() { }
    protected ICalendarException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ICalendarException(string? message) : base(message) { }
    public ICalendarException(string? message, Exception? innerException) : base(message, innerException) { }
}