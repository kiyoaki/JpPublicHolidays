using MasterMemory;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JpPublicHolidays
{
    /// <summary>
    /// Provides an in-memory database for holiday data using MasterMemory.
    /// Supports building from downloaded CSV, saving/loading to file, and searching by date and name.
    /// </summary>
    public class HolidayDatabase
    {
        private MemoryDatabase? _database;

        /// <summary>
        /// Gets the underlying MemoryDatabase instance.
        /// </summary>
        public MemoryDatabase? Database => _database;

        /// <summary>
        /// Gets whether the database has been initialized.
        /// </summary>
        public bool IsInitialized => _database != null;

        /// <summary>
        /// Builds the database from an array of Holiday objects.
        /// </summary>
        /// <param name="holidays">The holidays to add to the database.</param>
        public void Build(Holiday[] holidays)
        {
            if (holidays == null)
            {
                throw new ArgumentNullException(nameof(holidays));
            }
            var records = holidays.Select(HolidayRecord.FromHoliday).ToArray();
            var builder = new DatabaseBuilder();
            builder.Append(records);
            var data = builder.Build();
            _database = new MemoryDatabase(data);
        }

        /// <summary>
        /// Downloads holiday data from the public API and builds the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task BuildFromApiAsync(CancellationToken cancellationToken = default)
        {
            var holidays = await PublicHolidays.Get(cancellationToken).ConfigureAwait(false);
            Build(holidays);
        }

        /// <summary>
        /// Saves the database binary to a file.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        public void Save(string filePath)
        {
            if (_database == null)
            {
                throw new InvalidOperationException("Database is not initialized. Call Build or Load first.");
            }

            var builder = _database.ToDatabaseBuilder();
            var data = builder.Build();
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Saves the database binary to a stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Save(Stream stream)
        {
            if (_database == null)
            {
                throw new InvalidOperationException("Database is not initialized. Call Build or Load first.");
            }

            var builder = _database.ToDatabaseBuilder();
            builder.WriteToStream(stream);
        }

        /// <summary>
        /// Loads the database from a file.
        /// </summary>
        /// <param name="filePath">The file path to load from.</param>
        public void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }
            var data = File.ReadAllBytes(filePath);
            _database = new MemoryDatabase(data);
        }

        /// <summary>
        /// Loads the database from a byte array.
        /// </summary>
        /// <param name="data">The binary data to load.</param>
        public void Load(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            _database = new MemoryDatabase(data);
        }

        /// <summary>
        /// Loads the database from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from. The stream must be positioned at the start of the database data.</param>
        public void Load(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var data = ms.ToArray();
            _database = new MemoryDatabase(data);
        }

        /// <summary>
        /// Finds a holiday by its exact date.
        /// </summary>
        /// <param name="date">The date to search for.</param>
        /// <returns>The holiday on the specified date, or null if not found.</returns>
        public Holiday? FindByDate(DateTime date)
        {
            EnsureInitialized();
            var record = _database!.HolidayRecordTable.FindByDate(date);
            return record?.ToHoliday();
        }

        /// <summary>
        /// Tries to find a holiday by its exact date.
        /// </summary>
        /// <param name="date">The date to search for.</param>
        /// <param name="holiday">The found holiday, or null if not found.</param>
        /// <returns>True if found, false otherwise.</returns>
        public bool TryFindByDate(DateTime date, out Holiday? holiday)
        {
            EnsureInitialized();
            if (_database!.HolidayRecordTable.TryFindByDate(date, out var record))
            {
                holiday = record.ToHoliday();
                return true;
            }
            holiday = null;
            return false;
        }

        /// <summary>
        /// Finds all holidays with the specified name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>An array of holidays with the specified name.</returns>
        public Holiday[] FindByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            EnsureInitialized();
            var records = _database!.HolidayRecordTable.FindByName(name);
            return records.Select(r => r.ToHoliday()).ToArray();
        }

        /// <summary>
        /// Finds all holidays within a date range (inclusive).
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An array of holidays within the range.</returns>
        public Holiday[] FindByDateRange(DateTime startDate, DateTime endDate)
        {
            EnsureInitialized();
            var records = _database!.HolidayRecordTable.FindRangeByDate(startDate, endDate, true);
            return records.Select(r => r.ToHoliday()).ToArray();
        }

        /// <summary>
        /// Finds the closest holiday to the specified date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="selectLower">If true, selects the nearest holiday before the date; otherwise, after.</param>
        /// <returns>The closest holiday, or null if no holidays exist.</returns>
        public Holiday? FindClosestByDate(DateTime date, bool selectLower = true)
        {
            EnsureInitialized();
            var record = _database!.HolidayRecordTable.FindClosestByDate(date, selectLower);
            return record?.ToHoliday();
        }

        /// <summary>
        /// Gets all holidays in the database.
        /// </summary>
        /// <returns>An array of all holidays.</returns>
        public Holiday[] GetAll()
        {
            EnsureInitialized();
            return _database!.HolidayRecordTable.All.Select(r => r.ToHoliday()).ToArray();
        }

        /// <summary>
        /// Gets the total count of holidays in the database.
        /// </summary>
        public int Count
        {
            get
            {
                EnsureInitialized();
                return _database!.HolidayRecordTable.Count;
            }
        }

        /// <summary>
        /// Checks if the specified date is a holiday.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is a holiday, false otherwise.</returns>
        public bool IsHoliday(DateTime date)
        {
            return FindByDate(date) != null;
        }

        private void EnsureInitialized()
        {
            if (_database == null)
            {
                throw new InvalidOperationException("Database is not initialized. Call Build or Load first.");
            }
        }

        /// <summary>
        /// Creates and builds a new HolidayDatabase from an array of holidays.
        /// </summary>
        /// <param name="holidays">The holidays to add to the database.</param>
        /// <returns>A new initialized HolidayDatabase.</returns>
        public static HolidayDatabase Create(Holiday[] holidays)
        {
            var db = new HolidayDatabase();
            db.Build(holidays);
            return db;
        }

        /// <summary>
        /// Creates and builds a new HolidayDatabase by downloading holiday data from the public API.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A new initialized HolidayDatabase.</returns>
        public static async Task<HolidayDatabase> CreateFromApiAsync(CancellationToken cancellationToken = default)
        {
            var db = new HolidayDatabase();
            await db.BuildFromApiAsync(cancellationToken).ConfigureAwait(false);
            return db;
        }

        /// <summary>
        /// Creates and loads a HolidayDatabase from a file.
        /// </summary>
        /// <param name="filePath">The file path to load from.</param>
        /// <returns>A new HolidayDatabase loaded from the file.</returns>
        public static HolidayDatabase LoadFromFile(string filePath)
        {
            var db = new HolidayDatabase();
            db.Load(filePath);
            return db;
        }

        /// <summary>
        /// Creates and loads a HolidayDatabase from binary data.
        /// </summary>
        /// <param name="data">The binary data to load.</param>
        /// <returns>A new HolidayDatabase loaded from the data.</returns>
        public static HolidayDatabase LoadFromBytes(byte[] data)
        {
            var db = new HolidayDatabase();
            db.Load(data);
            return db;
        }

        /// <summary>
        /// Creates and loads a HolidayDatabase from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A new HolidayDatabase loaded from the stream.</returns>
        public static HolidayDatabase LoadFromStream(Stream stream)
        {
            var db = new HolidayDatabase();
            db.Load(stream);
            return db;
        }
    }
}
