using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Sharc.Core.ExtensionMethods;
using Sharc.Core.Models.Enums;
using Sharc.Core.Models.Structs;

namespace Sharc.Core.Models.Entities; 

public class RecurrenceRule {

    public Frequency Frequency { get; set; }
    public DateTime? Until { get; set; }
    public int? Count { get; set; }
    public int? Interval { get; set; }
    public Weekday? WeekStart { get; set; }
    
    [Column]
    private string? _bySecond;
    [Column]
    private string? _byMinute;
    [Column]
    private string? _byHour;
    [Column]
    private string? _byDay;
    [Column]
    private string? _byMonthDay;
    [Column]
    private string? _byYearDay;
    [Column]
    private string? _byWeekNo;
    [Column]
    private string? _byMonth;
    [Column]
    private string? _bySetPos;
    
    
    [NotMapped]
    public int[]? BySecond {
        get => _bySecond?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _bySecond = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByMinute {
        get => _byMinute?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byMinute = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByHour {
        get => _byHour?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byHour = string.Join(',', value);
        }
    }
    [NotMapped]
    public ByDayProperty[]? ByDay {
        get => _byDay?.Split(',').Select(ByDayProperty.Parse).ToArray();
        set {
            if(value is null)
                return;
            _byDay = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByMonthDay {
        get => _byMonthDay?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byMonthDay = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByYearDay {
        get => _byYearDay?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byYearDay = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByWeekNo {
        get => _byWeekNo?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byWeekNo = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? ByMonth {
        get => _byMonth?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _byMonth = string.Join(',', value);
        }
    }
    [NotMapped]
    public int[]? BySetPos {
        get => _bySetPos?.Split(",").Select(int.Parse).ToArray();
        set {
            if (value is null)
                return;
            _bySetPos = string.Join(',', value);
        }
    }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append($"RRULE:FREQ={Frequency}");
        
        // either Until or Count is allowed, not both
        if (Until is not null && Count is null) {
            builder.Append($";UNTIL={Until.Value.ToRFCTimeString()}");
        }else if (Count is not null) {
            builder.Append($";COUNT={Count.Value}");
        }

        if (Interval is not null) {
            builder.Append($";INTERVAL={Interval.Value}");
        }

        AddArrayToBuilder(builder, "BYSECOND", _bySecond);
        AddArrayToBuilder(builder, "BYMINUTE", _byMinute);
        AddArrayToBuilder(builder, "BYHOUR", _byHour);
        AddArrayToBuilder(builder, "BYDAY", _byDay);
        AddArrayToBuilder(builder, "BYMONTHDAY", _byMonthDay);
        AddArrayToBuilder(builder, "BYYEARDAY", _byYearDay);
        AddArrayToBuilder(builder, "BYWEEKNO", _byWeekNo);
        AddArrayToBuilder(builder, "BYMONTH", _byMonth);
        AddArrayToBuilder(builder, "BYSETPOS", _bySetPos);

        if (WeekStart is not null) {
            builder.Append($";WKST={WeekStart.Value}");
        }

        return builder.ToString();
    }

    private static void AddArrayToBuilder(StringBuilder builder, string attrib, string? val) {
        if (val is null || val.Length == 0)
            return;
        builder.Append($";{attrib}=");
        builder.Append(val);
    }
}