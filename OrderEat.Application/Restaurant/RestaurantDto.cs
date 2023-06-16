namespace OrderEat.Application.Restaurant
{
    public class RestaurantDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string ApartmentNumber { get; set; } = default!;
    }
}