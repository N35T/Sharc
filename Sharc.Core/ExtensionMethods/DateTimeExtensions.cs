namespace Sharc.Core.ExtensionMethods; 

public static class DateTimeExtensions {
    
    public static string ToRFCTimeString(this DateTime time, bool isDateOnly = false) {
        if(isDateOnly) {
            return time.ToUniversalTime().ToString("yyyyMMdd");
        }
        return time.ToUniversalTime().ToString("yyyyMMddTHHmmss");
    }
}