namespace Morninger.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}