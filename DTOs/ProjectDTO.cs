using System.ComponentModel.DataAnnotations;

namespace Practice.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Code { get; set; }

        public bool IsActive { get; set; }
    }
}
