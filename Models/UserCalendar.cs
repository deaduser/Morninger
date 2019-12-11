namespace Morninger.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class UserCalendar
    {
        [Key]
        public int Id { get; set; }
        public List<UserDay> Days { get; set; }
    }
}