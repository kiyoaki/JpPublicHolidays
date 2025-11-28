using JpPublicHolidays;

Console.WriteLine("=== JpPublicHolidays with MasterMemory Demo ===\n");

// Create sample holiday data (simulating data that would be downloaded from the CSV)
var sampleHolidays = new Holiday[]
{
    new() { Date = new DateTime(2025, 1, 1), Name = "元日" },
    new() { Date = new DateTime(2025, 1, 13), Name = "成人の日" },
    new() { Date = new DateTime(2025, 2, 11), Name = "建国記念の日" },
    new() { Date = new DateTime(2025, 2, 23), Name = "天皇誕生日" },
    new() { Date = new DateTime(2025, 2, 24), Name = "休日" },
    new() { Date = new DateTime(2025, 3, 20), Name = "春分の日" },
    new() { Date = new DateTime(2025, 4, 29), Name = "昭和の日" },
    new() { Date = new DateTime(2025, 5, 3), Name = "憲法記念日" },
    new() { Date = new DateTime(2025, 5, 4), Name = "みどりの日" },
    new() { Date = new DateTime(2025, 5, 5), Name = "こどもの日" },
    new() { Date = new DateTime(2025, 5, 6), Name = "休日" },
    new() { Date = new DateTime(2024, 1, 1), Name = "元日" },
    new() { Date = new DateTime(2024, 2, 23), Name = "天皇誕生日" },
};

// Create and build the holiday database
Console.WriteLine("1. Creating HolidayDatabase from holiday data...");
var db = HolidayDatabase.Create(sampleHolidays);
Console.WriteLine($"   Database created with {db.Count} holidays.\n");

// Save the database to a file
var dbFilePath = Path.Combine(Path.GetTempPath(), "holidays.db");
Console.WriteLine($"2. Saving database to: {dbFilePath}");
db.Save(dbFilePath);
Console.WriteLine("   Database saved successfully.\n");

// Load the database from file
Console.WriteLine("3. Loading database from file...");
var loadedDb = HolidayDatabase.LoadFromFile(dbFilePath);
Console.WriteLine($"   Database loaded with {loadedDb.Count} holidays.\n");

// Search by exact date
Console.WriteLine("4. Searching by date (2025/1/1)...");
var holiday = loadedDb.FindByDate(new DateTime(2025, 1, 1));
if (holiday != null)
{
    Console.WriteLine($"   Found: {holiday.Name} ({holiday.Date:yyyy/MM/dd})\n");
}

// Search by name
Console.WriteLine("5. Searching by name ('元日')...");
var holidays = loadedDb.FindByName("元日");
Console.WriteLine($"   Found {holidays.Length} holidays:");
foreach (var h in holidays)
{
    Console.WriteLine($"   - {h.Name} ({h.Date:yyyy/MM/dd})");
}
Console.WriteLine();

// Search by date range
Console.WriteLine("6. Searching by date range (2025/4/1 - 2025/5/31)...");
var rangeHolidays = loadedDb.FindByDateRange(
    new DateTime(2025, 4, 1),
    new DateTime(2025, 5, 31));
Console.WriteLine($"   Found {rangeHolidays.Length} holidays:");
foreach (var h in rangeHolidays)
{
    Console.WriteLine($"   - {h.Name} ({h.Date:yyyy/MM/dd})");
}
Console.WriteLine();

// Check if a date is a holiday
Console.WriteLine("7. Checking if dates are holidays...");
var testDates = new[]
{
    new DateTime(2025, 1, 1),
    new DateTime(2025, 1, 2),
    new DateTime(2025, 5, 5)
};
foreach (var date in testDates)
{
    var isHoliday = loadedDb.IsHoliday(date);
    Console.WriteLine($"   {date:yyyy/MM/dd}: {(isHoliday ? "祝日です" : "祝日ではありません")}");
}
Console.WriteLine();

// Find closest holiday
Console.WriteLine("8. Finding closest holiday to 2025/2/1...");
var closest = loadedDb.FindClosestByDate(new DateTime(2025, 2, 1), selectLower: false);
if (closest != null)
{
    Console.WriteLine($"   Closest holiday after: {closest.Name} ({closest.Date:yyyy/MM/dd})");
}
closest = loadedDb.FindClosestByDate(new DateTime(2025, 2, 1), selectLower: true);
if (closest != null)
{
    Console.WriteLine($"   Closest holiday before: {closest.Name} ({closest.Date:yyyy/MM/dd})");
}
Console.WriteLine();

// Get all holidays
Console.WriteLine("9. All holidays in the database:");
foreach (var h in loadedDb.GetAll())
{
    Console.WriteLine($"   {h.Name} ({h.Date:yyyy/MM/dd})");
}

// Clean up
File.Delete(dbFilePath);
Console.WriteLine("\n=== Demo completed ===");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();