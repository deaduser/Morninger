namespace Edomozh
{
    using System;
    using System.Collections.Generic;

    internal class User
    {
        internal User()
        {
            Entries = new List<Entry>();
        }

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public List<Entry> Entries { get; set; }
    }
}