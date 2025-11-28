namespace JpPublicHolidays.Test.Net8
{
    public class HolidayDatabaseTest
    {
        private Holiday[] GetSampleHolidays()
        {
            return new Holiday[]
            {
                new() { Date = new DateTime(2025, 1, 1), Name = "元日" },
                new() { Date = new DateTime(2025, 1, 13), Name = "成人の日" },
                new() { Date = new DateTime(2025, 2, 11), Name = "建国記念の日" },
                new() { Date = new DateTime(2025, 2, 23), Name = "天皇誕生日" },
                new() { Date = new DateTime(2025, 5, 3), Name = "憲法記念日" },
                new() { Date = new DateTime(2025, 5, 4), Name = "みどりの日" },
                new() { Date = new DateTime(2025, 5, 5), Name = "こどもの日" },
                new() { Date = new DateTime(2024, 1, 1), Name = "元日" },
            };
        }

        [Fact]
        public void Create_ShouldBuildDatabaseFromHolidays()
        {
            // Arrange
            var holidays = GetSampleHolidays();

            // Act
            var db = HolidayDatabase.Create(holidays);

            // Assert
            Assert.True(db.IsInitialized);
            Assert.Equal(holidays.Length, db.Count);
        }

        [Fact]
        public void FindByDate_ShouldReturnCorrectHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var searchDate = new DateTime(2025, 1, 1);

            // Act
            var result = db.FindByDate(searchDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("元日", result.Name);
            Assert.Equal(searchDate, result.Date);
        }

        [Fact]
        public void FindByDate_ShouldReturnNullForNonHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var searchDate = new DateTime(2025, 1, 2);

            // Act
            var result = db.FindByDate(searchDate);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryFindByDate_ShouldReturnTrueForHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var searchDate = new DateTime(2025, 1, 1);

            // Act
            var found = db.TryFindByDate(searchDate, out var result);

            // Assert
            Assert.True(found);
            Assert.NotNull(result);
            Assert.Equal("元日", result.Name);
        }

        [Fact]
        public void TryFindByDate_ShouldReturnFalseForNonHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var searchDate = new DateTime(2025, 1, 2);

            // Act
            var found = db.TryFindByDate(searchDate, out var result);

            // Assert
            Assert.False(found);
            Assert.Null(result);
        }

        [Fact]
        public void FindByName_ShouldReturnAllMatchingHolidays()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());

            // Act
            var results = db.FindByName("元日");

            // Assert
            Assert.Equal(2, results.Length);
            Assert.All(results, h => Assert.Equal("元日", h.Name));
        }

        [Fact]
        public void FindByName_ShouldReturnEmptyArrayForNonExistentName()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());

            // Act
            var results = db.FindByName("存在しない祝日");

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void FindByDateRange_ShouldReturnHolidaysWithinRange()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var startDate = new DateTime(2025, 5, 1);
            var endDate = new DateTime(2025, 5, 31);

            // Act
            var results = db.FindByDateRange(startDate, endDate);

            // Assert
            Assert.Equal(3, results.Length);
            Assert.All(results, h => Assert.True(h.Date >= startDate && h.Date <= endDate));
        }

        [Fact]
        public void FindClosestByDate_ShouldFindNearestHolidayBefore()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var referenceDate = new DateTime(2025, 2, 1);

            // Act
            var result = db.FindClosestByDate(referenceDate, selectLower: true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("成人の日", result.Name);
            Assert.Equal(new DateTime(2025, 1, 13), result.Date);
        }

        [Fact]
        public void FindClosestByDate_ShouldFindNearestHolidayAfter()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());
            var referenceDate = new DateTime(2025, 2, 1);

            // Act
            var result = db.FindClosestByDate(referenceDate, selectLower: false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("建国記念の日", result.Name);
            Assert.Equal(new DateTime(2025, 2, 11), result.Date);
        }

        [Fact]
        public void IsHoliday_ShouldReturnTrueForHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());

            // Act & Assert
            Assert.True(db.IsHoliday(new DateTime(2025, 1, 1)));
            Assert.True(db.IsHoliday(new DateTime(2025, 5, 5)));
        }

        [Fact]
        public void IsHoliday_ShouldReturnFalseForNonHoliday()
        {
            // Arrange
            var db = HolidayDatabase.Create(GetSampleHolidays());

            // Act & Assert
            Assert.False(db.IsHoliday(new DateTime(2025, 1, 2)));
            Assert.False(db.IsHoliday(new DateTime(2025, 12, 25)));
        }

        [Fact]
        public void GetAll_ShouldReturnAllHolidays()
        {
            // Arrange
            var holidays = GetSampleHolidays();
            var db = HolidayDatabase.Create(holidays);

            // Act
            var results = db.GetAll();

            // Assert
            Assert.Equal(holidays.Length, results.Length);
        }

        [Fact]
        public void SaveAndLoad_ShouldPreserveData()
        {
            // Arrange
            var holidays = GetSampleHolidays();
            var db = HolidayDatabase.Create(holidays);
            var filePath = Path.Combine(Path.GetTempPath(), $"test_holidays_{Guid.NewGuid()}.db");

            try
            {
                // Act
                db.Save(filePath);
                var loadedDb = HolidayDatabase.LoadFromFile(filePath);

                // Assert
                Assert.Equal(db.Count, loadedDb.Count);

                var holiday = loadedDb.FindByDate(new DateTime(2025, 1, 1));
                Assert.NotNull(holiday);
                Assert.Equal("元日", holiday.Name);
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Fact]
        public void SaveToStream_AndLoadFromBytes_ShouldPreserveData()
        {
            // Arrange
            var holidays = GetSampleHolidays();
            var db = HolidayDatabase.Create(holidays);

            // Act
            byte[] data;
            using (var ms = new MemoryStream())
            {
                db.Save(ms);
                data = ms.ToArray();
            }

            var loadedDb = HolidayDatabase.LoadFromBytes(data);

            // Assert
            Assert.Equal(db.Count, loadedDb.Count);
            Assert.NotNull(loadedDb.FindByDate(new DateTime(2025, 1, 1)));
        }

        [Fact]
        public void SaveToStream_AndLoadFromStream_ShouldPreserveData()
        {
            // Arrange
            var holidays = GetSampleHolidays();
            var db = HolidayDatabase.Create(holidays);

            // Act
            using var saveStream = new MemoryStream();
            db.Save(saveStream);
            saveStream.Position = 0;

            var loadedDb = HolidayDatabase.LoadFromStream(saveStream);

            // Assert
            Assert.Equal(db.Count, loadedDb.Count);
            Assert.NotNull(loadedDb.FindByDate(new DateTime(2025, 1, 1)));
        }

        [Fact]
        public void FindByDate_ThrowsWhenNotInitialized()
        {
            // Arrange
            var db = new HolidayDatabase();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => db.FindByDate(DateTime.Now));
        }

        [Fact]
        public void Save_ThrowsWhenNotInitialized()
        {
            // Arrange
            var db = new HolidayDatabase();
            var filePath = Path.Combine(Path.GetTempPath(), "test.db");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => db.Save(filePath));
        }

        [Fact]
        public async Task CreateFromApiAsync_ShouldBuildDatabase()
        {
            // Act
            var db = await HolidayDatabase.CreateFromApiAsync();

            // Assert
            Assert.True(db.IsInitialized);
            Assert.True(db.Count > 0);
            var newYear = db.FindByDate(new DateTime(DateTime.Now.Year, 1, 1));
            Assert.NotNull(newYear);
            Assert.Equal("元日", newYear.Name);
        }
    }
}
