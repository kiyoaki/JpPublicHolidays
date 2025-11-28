using MasterMemory;
using MessagePack;
using System;

namespace JpPublicHolidays
{
    /// <summary>
    /// MasterMemory table definition for holidays.
    /// Supports searching by date (primary key) and by name (secondary key).
    /// </summary>
    [MemoryTable("holiday"), MessagePackObject(true)]
    public record HolidayRecord
    {
        /// <summary>
        /// Primary key: The date of the holiday.
        /// </summary>
        [PrimaryKey]
        public DateTime Date { get; init; }

        /// <summary>
        /// Secondary key (non-unique): The name of the holiday.
        /// Allows searching holidays by name.
        /// </summary>
        [SecondaryKey(0), NonUnique]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Converts a Holiday object to a HolidayRecord.
        /// </summary>
        public static HolidayRecord FromHoliday(Holiday holiday)
        {
            return new HolidayRecord
            {
                Date = holiday.Date,
                Name = holiday.Name
            };
        }

        /// <summary>
        /// Converts this HolidayRecord to a Holiday object.
        /// </summary>
        public Holiday ToHoliday()
        {
            return new Holiday
            {
                Date = Date,
                Name = Name
            };
        }
    }
}
