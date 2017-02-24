using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JpPublicHolidays.Test
{
    [TestClass]
    public class PublicHolidaysApiTest
    {
        [TestMethod]
        public async Task TestGet()
        {
            var holidays = await PublicHolidays.Get();
            Assert.IsTrue(holidays.Length >= 20);

            var day = holidays.FirstOrDefault(x => x.Date == new DateTime(DateTime.Now.Year, 1, 1));
            Assert.IsNotNull(day);
            Assert.AreEqual(day.Name, "元日");
        }
    }
}
