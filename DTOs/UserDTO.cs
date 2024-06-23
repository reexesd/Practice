using System.ComponentModel.DataAnnotations;

namespace Practice.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;
    }
}
