using Practice.Interfaces;

namespace Practice.Models
{
    public class Project: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive {  get; set; }

        public List<Task> Tasks { get; set; } = [];
    }
}
