namespace Morninger
{
    using System;
    using System.Collections.Generic;

    internal class User
    {
        internal User()
        {
            Statistic = new List<Month>();
        }

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public DateTime LastUpdate { get; set; }

        public List<Month> Statistic { get; set; }
    }

    internal class Month
    {

        public long UserId { get; set; }

        public int Year { get; set; }

        public int Number { get; set; }

        public int Done { get; set; } = 0;

        public int DayOff { get; set; } = 0;

        public int Undone
        {
            get
            {
                return DateTime.UtcNow.Day - Done - DayOff;
            }
        }
    }
}