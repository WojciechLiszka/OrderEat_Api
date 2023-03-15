namespace FastFood.Application.Account
{
    public class UserDto
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime DateofBirth { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string ApartmentNumber { get; set; } = default!;
    }
}