using System;
using System.Threading.Tasks;

namespace JpPublicHolidays.ConsoleApp472
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var holidays = await PublicHolidays.Get();
            foreach (var holiday in holidays)
            {
                Console.WriteLine($"{holiday.Name} {holiday.Date.ToShortDateString()}");
            }
            Console.ReadKey();
        }
    }
}