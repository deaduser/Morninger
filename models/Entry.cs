namespace Edomozh
{
    using System;

    internal class Entry
    {
        internal Entry(long userId)
        {
            UserId = userId;
            Year = DateTime.UtcNow.Year;
            Month = DateTime.UtcNow.Month;
            Day = DateTime.UtcNow.Day;
        }

        public long UserId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public string Status { get; set; }
    }
}