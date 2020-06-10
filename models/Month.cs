namespace Edomozh
{
    using System;

    internal class Month
    {
        internal Month(long userId)
        {
            UserId = userId;
        }

        public long UserId { get; set; }

        public int Year { get; set; } = DateTime.UtcNow.Year;

        public int Number { get; set; } = DateTime.UtcNow.Month;

        public int Done { get; set; } = 0;

        public int DayOff { get; set; } = 0;

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public int Undone => DateTime.UtcNow.Day - Done - DayOff;

    }
}