namespace CompetitorsBot.Bot.Models
{
    using System.Collections.Generic;

    public class User : DbObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}
