using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            Timeout = TimeSpan.FromSeconds(60)
        };

        static PublicHolidays()
        {
#if !NET45
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        public static async Task<Holiday[]> Get(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var response = await HttpClient.GetAsync(Path, cancellationToken).ConfigureAwait(false);
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
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
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.RequestCanceled:
                    case WebExceptionStatus.Timeout:
                        throw new PublicHolidaysException("Request Timeout");
                    default:
                        throw;
                }
            }
            catch (TaskCanceledException)
            {
                throw new PublicHolidaysException("Request Timeout");
            }
            catch (OperationCanceledException)
            {
                throw new PublicHolidaysException("Request Timeout");
            }
        }
    }
}
