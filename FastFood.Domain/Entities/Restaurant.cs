namespace FastFood.Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public RestaurantContactDetails ContactDetails { get; set; }

    }
}