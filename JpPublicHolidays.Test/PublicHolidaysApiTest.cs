using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PublicHolidays.Test
{
    [TestClass]
    public class PublicHolidaysApiTest
    {
        [TestMethod]
        public void TestGet()
        {
            var holidays = JpPublicHolidays.PublicHolidays.Get().Result;
            Assert.IsTrue(holidays.Length >= 20);

            var day = holidays.FirstOrDefault(x => x.Date == new DateTime(DateTime.Now.Year, 1, 1));
            Assert.IsNotNull(day);
            Assert.AreEqual(day.Name, "元日");
        }
    }
}
