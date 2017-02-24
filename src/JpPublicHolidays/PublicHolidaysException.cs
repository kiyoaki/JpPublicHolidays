using System;

namespace PublicHolidays
{
    public class PublicHolidaysException : Exception
    {
        public PublicHolidaysException(string message)
            : base(message)
        {
        }
    }
}
