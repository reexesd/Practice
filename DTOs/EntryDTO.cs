using System.ComponentModel.DataAnnotations;

namespace Practice.DTOs
{
    public class EntryDTO
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public int Hours { get; set; }

        [Required]
        public required string Description { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }
    }
}
