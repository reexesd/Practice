using System.ComponentModel.DataAnnotations;
using Practice.Interfaces;

namespace Practice.Models
{
    public class User: IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public List<Entry> Entries { get; set; } = [];
    }
}
