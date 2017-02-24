using System;

namespace JpPublicHolidays
{
    public class PublicHolidaysException : Exception
    {
        public PublicHolidaysException(string message)
            : base(message)
        {
        }
    }
}
