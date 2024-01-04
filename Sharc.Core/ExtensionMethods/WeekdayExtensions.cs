using Sharc.Core.Models.Enums;

namespace Sharc.Core.ExtensionMethods; 

public static class WeekdayExtensions {

    public static Weekday Parse(string s) {
        switch (s.ToLower()) {
            case "monday":
            case "mo":
                return Weekday.MONDAY;    
            case "tuesday":
            case "tu":
                return Weekday.TUESDAY;
            case "wednesday":
            case "we":
                return Weekday.WEDNESDAY;
            case "thursday":
            case "th":
                return Weekday.THURSDAY;
            case "friday":
            case "fr":
                return Weekday.FRIDAY;
            case "saturday":
            case "sa":
                return Weekday.SATURDAY;
            case "sunday":
            case "su":
                return Weekday.SUNDAY;
            default:
                throw new ArgumentException();
        }
    }

    public static string ToShortString(this Weekday day) {
        return day.ToString()[0..2];
    }
}