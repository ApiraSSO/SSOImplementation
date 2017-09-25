using System;

namespace Kernel.Extensions
{
    public static class DataTimeExtensions
    {
        public static DateTime Add(this DateTime time, TimeSpan timespan)
        {
            if (timespan == TimeSpan.Zero)
                return time;
            if (timespan > TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
                return DataTimeExtensions.GetMaxValue(time.Kind);
            if (timespan < TimeSpan.Zero && DateTime.MinValue - time >= timespan)
                return DataTimeExtensions.GetMinValue(time.Kind);
            return time + timespan;
        }

        public static DateTime GetMaxValue(DateTimeKind kind)
        {
            if (kind == DateTimeKind.Unspecified)
                return new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc);
            return new DateTime(DateTime.MaxValue.Ticks, kind);
        }

        public static DateTime GetMinValue(DateTimeKind kind)
        {
            if (kind == DateTimeKind.Unspecified)
                return new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc);
            return new DateTime(DateTime.MinValue.Ticks, kind);
        }

        public static DateTime? ToUniversalTime(this DateTime? value)
        {
            if (!value.HasValue || value.Value.Kind == DateTimeKind.Utc)
                return value;
            return new DateTime?(DataTimeExtensions.ToUniversalTime(value.Value));
        }

        public static DateTime ToUniversalTime(this DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;
            return value.ToUniversalTime();
        }
    }
}