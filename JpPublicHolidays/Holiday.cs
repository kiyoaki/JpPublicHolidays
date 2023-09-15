using System;

namespace JpPublicHolidays
{
    public class Holiday
    {
        private static readonly TimeZoneInfo JstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

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
