using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string Phone { get; set; }
    }
}