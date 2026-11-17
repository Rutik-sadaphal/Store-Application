using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}