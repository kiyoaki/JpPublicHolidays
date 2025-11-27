using System;

namespace JpPublicHolidays
{
    public class Holiday
    {
        private static readonly TimeZoneInfo JstTimeZoneInfo;

        static Holiday()
        {
            try
            {
                // Windows uses "Tokyo Standard Time"
                JstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Linux/macOS uses "Asia/Tokyo"
                JstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            }
        }

        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTimeOffset DateWithTimeZone
        {
            get
            {
                return new DateTimeOffset(Date, JstTimeZoneInfo.BaseUtcOffset);
            }
        }
    }
}
