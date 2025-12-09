using NextUnit;

namespace JpPublicHolidays.Test
{
    public class PublicHolidaysApiTest
    {
        [Test]
        public async Task TestGet()
        {
            var holidays = await PublicHolidays.Get();
            Assert.True(holidays.Length >= 20);

            var day = holidays.FirstOrDefault(x => x.Date == new DateTime(DateTime.Now.Year, 1, 1));
            Assert.NotNull(day);
            Assert.Equal("元日", day?.Name);
        }
    }
}
