using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace PublicHolidays
{
    internal class CsvParser : IDisposable
    {
        private readonly CsvReader _csvReader;

        internal CsvParser(TextReader streamReader)
        {
            _csvReader = new CsvReader(streamReader, new CsvConfiguration
            {
                CultureInfo = new CultureInfo("ja-JP")
            });
        }

        internal Holiday[] Parse()
        {
            var list = new List<Holiday>();
            while (_csvReader.Read())
            {
                try
                {
                    string name;
                    DateTime date;
                    if (_csvReader.TryGetField(0, out name) && _csvReader.TryGetField(1, out date))
                    {
                        list.Add(new Holiday
                        {
                            Name = name,
                            Date = date
                        });
                    }

                    if (_csvReader.TryGetField(2, out name) && _csvReader.TryGetField(3, out date))
                    {
                        list.Add(new Holiday
                        {
                            Name = name,
                            Date = date
                        });
                    }

                    if (_csvReader.TryGetField(4, out name) && _csvReader.TryGetField(5, out date))
                    {
                        list.Add(new Holiday
                        {
                            Name = name,
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

    internal class Record
    {
        public string Name1 { get; set; }
        public DateTime? Date1 { get; set; }

        public string Name2 { get; set; }
        public DateTime? Date2 { get; set; }

        public string Name3 { get; set; }
        public DateTime? Date3 { get; set; }
    }
}
