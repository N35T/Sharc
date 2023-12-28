namespace Sharc.Core.ExtensionMethods; 

public static class DateTimeExtensions {
    
    public static string ToRFCTimeString(this DateTime time) {
        return time.ToUniversalTime().ToString("yyyyMMddTHHmmss");
    }
}