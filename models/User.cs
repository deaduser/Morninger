namespace Edomozh
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
}