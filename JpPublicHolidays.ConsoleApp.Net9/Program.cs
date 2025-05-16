var holidays = await PublicHolidays.Get();
foreach (var holiday in holidays)
{
    Console.WriteLine($"{holiday.Name} {holiday.Date.ToShortDateString()}");
}
Console.ReadKey();