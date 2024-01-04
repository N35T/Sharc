using System.Runtime.Serialization;

namespace Sharc.Core.Exceptions; 

public class EventOperationException : Exception {
    public EventOperationException() { }
    protected EventOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public EventOperationException(string? message) : base(message) { }
    public EventOperationException(string? message, Exception? innerException) : base(message, innerException) { }
}