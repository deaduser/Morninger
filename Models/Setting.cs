namespace Morninger.Models
{
    using System.ComponentModel.DataAnnotations;
    internal class Setting
    {
        [Key]
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal string Value { get; set; }
        internal string Description { get; set; }
    }
}