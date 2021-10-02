namespace CompetitorsBot.Bot.Models
{
    public class Entry : DbObject
    {
        public User User { get; set; }

        public string Status { get; set; }
    }
}
