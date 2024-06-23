using Practice.Interfaces;

namespace Practice.Models
{
    public class Entry: IEntity
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public int Hours { get; set; }
        public string Description { get; set; } = null!;
        public Task Task { get; set; } = null!;
        public int TaskId {  get; set; }
        public User User { get; set; } = null!;
        public int UserId { get; set; }
    }
}
