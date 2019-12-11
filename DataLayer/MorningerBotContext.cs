namespace Morninger.DataLayer
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Models;

    internal class MorningerBotContext : DbContext
    {
        internal DbSet<User> Users { get; set; }
        internal DbSet<Setting> Settings { get; set; }
        internal DbSet<UserCalendar> UserCalendars { get; set; }
        internal DbSet<UserDay> UserDays { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=morningerbot.db");
    }
}