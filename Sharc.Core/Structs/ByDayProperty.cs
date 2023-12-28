using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using Sharc.Core.Enums;
using Sharc.Core.ExtensionMethods;

namespace Sharc.Core.Structs; 

public struct ByDayProperty {
    // 1Fr would be every first friday
    public Weekday Weekday { get; set; }
    public int? Ordinal { get; set; }

    public override string ToString() {
        if (Ordinal is null) {
            return Weekday.ToString();
        }
        return Ordinal.Value.ToString() + Weekday;
    }

    public static ByDayProperty Parse(string s) {
        var pointer = 0;
        while (pointer < s.Length && (s[pointer] == '-' || s[pointer] == '+' || char.IsDigit(s[pointer]))) {
            ++pointer;
        }
        if (pointer >= s.Length) {
            throw new ArgumentException();
        }
        var byDay = new ByDayProperty();
        if (pointer > 0) {
            byDay.Ordinal = int.Parse(s[0..pointer]);
        }

        byDay.Weekday = WeekdayExtensions.Parse(s[pointer..]);
        return byDay;
    }
}