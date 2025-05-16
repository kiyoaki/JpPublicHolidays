namespace JpPublicHolidays.Test.Net8
{
    public class PublicHolidaysApiTest
    {
        [Fact]
        public async Task TestGet()
        {
            var holidays = await PublicHolidays.Get();
            Assert.True(holidays.Length >= 20);

            var day = holidays.FirstOrDefault(x => x.Date == new DateTime(DateTime.Now.Year, 1, 1));
            Assert.NotNull(day);
            Assert.Equal("Œ³“ú", day.Name);
        }
    }
}