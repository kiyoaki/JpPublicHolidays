using NextUnit;

namespace JpPublicHolidays.Test
{
    public class HolidayTest
    {
        [Test]
        public void DateWithTimeZone_ShouldWorkAfterStaticInitialization()
        {
            // Act - Create a Holiday instance and access DateWithTimeZone
            var holiday = new Holiday
            {
                Name = "元日",
                Date = new DateTime(2025, 1, 1)
            };

            // Assert - DateWithTimeZone should not throw and return valid value
            var dateWithTimeZone = holiday.DateWithTimeZone;
            Assert.NotEqual(default, dateWithTimeZone);
        }

        [Test]
        public void DateWithTimeZone_ShouldReturnUtcPlus9Offset()
        {
            // Arrange
            var holiday = new Holiday
            {
                Name = "元日",
                Date = new DateTime(2025, 1, 1, 0, 0, 0)
            };

            // Act
            var dateWithTimeZone = holiday.DateWithTimeZone;

            // Assert - JST is UTC+9
            var expectedOffset = TimeSpan.FromHours(9);
            Assert.Equal(expectedOffset, dateWithTimeZone.Offset);
        }

        [Test]
        public void DateWithTimeZone_ShouldPreserveDateTime()
        {
            // Arrange
            var testDate = new DateTime(2025, 5, 5, 12, 30, 45);
            var holiday = new Holiday
            {
                Name = "こどもの日",
                Date = testDate
            };

            // Act
            var dateWithTimeZone = holiday.DateWithTimeZone;

            // Assert - The date/time portion should be preserved
            Assert.Equal(testDate.Year, dateWithTimeZone.Year);
            Assert.Equal(testDate.Month, dateWithTimeZone.Month);
            Assert.Equal(testDate.Day, dateWithTimeZone.Day);
            Assert.Equal(testDate.Hour, dateWithTimeZone.Hour);
            Assert.Equal(testDate.Minute, dateWithTimeZone.Minute);
            Assert.Equal(testDate.Second, dateWithTimeZone.Second);
        }
    }
}
