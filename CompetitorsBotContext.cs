namespace CompetitorsBot.Bot
{
    using CompetitorsBot.Bot.Models;
    using Microsoft.EntityFrameworkCore;

    public class CompetitorsBotContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=morninger.db");
    }
}
