using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace JpPublicHolidays
{
    internal class CsvParser : IDisposable
    {
        private readonly CsvReader _csvReader;

        internal CsvParser(TextReader streamReader)
        {
            _csvReader = new CsvReader(streamReader, new CultureInfo("ja-JP"));
        }

        internal Holiday[] Parse()
        {
            var list = new List<Holiday>();
            while (_csvReader.Read())
            {
                try
                {
                    if (_csvReader.TryGetField(0, out DateTime date) && _csvReader.TryGetField(1, out string? name))
                    {
                        list.Add(new Holiday
                        {
                            Name = name ?? string.Empty,
                            Date = date
                        });
                    }
                }
                catch
                {
                    //ignore
                }
            }

            return list.OrderBy(x => x.Date).ToArray();
        }

        public void Dispose()
        {
            _csvReader.Dispose();
        }
    }
}
