namespace CompetitorsBot.Bot.Models
{
    using System;

    public abstract class DbObject
    {
        public int Id { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdateDate { get; set; }

    }
}
