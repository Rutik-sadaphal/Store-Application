namespace StoreApplicationn.DTOs
{
    public class AddressResponseDto
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }
}