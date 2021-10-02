namespace CompetitorsBot.Bot.Models
{
    using System.Collections.Generic;

    public class Room : DbObject
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
