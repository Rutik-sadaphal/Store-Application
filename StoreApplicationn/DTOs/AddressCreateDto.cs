using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class AddressCreateDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public bool IsDefault { get; set; }
    }
}