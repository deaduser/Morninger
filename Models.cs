namespace Morninger
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    internal class User
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("LastUpdate")]
        public DateTime LastUpdate { get; set; }
        public List<UserMonth> Statistic { get; set; }

    }

    [Table("UserMonth")]
    internal class UserMonth
    {
        [Column("Id")]
        public int UserId { get; set; }
        [Column("Year")]
        public int Year { get; set; }
        [Column("Month")]
        public int Month { get; set; }
        [Column("Done")]
        public int Done { get; set; }
        [Column("DayOff")]
        public int DayOff { get; set; }
        public int Undone
        {
            get
            {
                return DateTime.UtcNow.Day - Done - DayOff;
            }
        }
    }
}