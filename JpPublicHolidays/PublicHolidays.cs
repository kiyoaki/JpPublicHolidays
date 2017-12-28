using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JpPublicHolidays
{
    public static class PublicHolidays
    {
        private static readonly Uri BaseUri = new Uri("http://www8.cao.go.jp/");
        private const string Path = "/chosei/shukujitsu/syukujitsu.csv";

        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = BaseUri,
            Timeout = TimeSpan.FromSeconds(10)
        };

        static PublicHolidays()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async Task<Holiday[]> Get()
        {
            try
            {
                var response = await HttpClient.GetAsync(Path);
                var stream = await response.Content.ReadAsStreamAsync();
                using (var streamReader = new StreamReader(stream, Encoding.GetEncoding("shift_jis"), true))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new PublicHolidaysException(
                            $"Status code: {response.StatusCode} Response data: {streamReader.ReadToEnd()}");
                    }

                    using (var csvParser = new CsvParser(streamReader))
                    {
                        return csvParser.Parse();
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw new PublicHolidaysException("Request Timeout");
            }
        }
    }
}
