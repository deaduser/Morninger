namespace Morninger.Models
{
    using Morninger.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class UserDay
    {
        [Key]
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public UserDailyStatus Status { get; set; }
    }
}