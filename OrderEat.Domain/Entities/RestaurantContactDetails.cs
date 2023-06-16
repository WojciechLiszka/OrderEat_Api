namespace OrderEat.Domain.Entities
{
    public class RestaurantContactDetails
    {
        public string ContactNumber { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string ApartmentNumber { get; set; } = default!;
    }
}