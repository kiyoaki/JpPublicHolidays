using System;

namespace JpPublicHolidays
{
    public class Holiday
    {
        private static readonly TimeZoneInfo JstTimeZoneInfo;
        private static readonly TimeSpan JstOffset = TimeSpan.FromHours(9);

        static Holiday()
        {
            try
            {
                // Windows uses "Tokyo Standard Time"
                JstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // Linux/macOS uses "Asia/Tokyo"
                    JstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
                }
                catch (TimeZoneNotFoundException)
                {
                    // Fallback: Create a custom timezone for JST (UTC+9)
                    JstTimeZoneInfo = TimeZoneInfo.CreateCustomTimeZone(
                        "JST",
                        JstOffset,
                        "Japan Standard Time",
                        "Japan Standard Time");
                }
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
