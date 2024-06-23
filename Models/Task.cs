using System.ComponentModel.DataAnnotations;
using Practice.Interfaces;

namespace Practice.Models
{
    public class Task: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
        public Project Project { get; set; } = null!;
        public List<Entry> Entries { get; set; } = [];
    }
}
