using System.ComponentModel.DataAnnotations;

namespace Practice.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public int ProjectId { get; set; }

        public bool IsActive { get; set; }
    }
}
