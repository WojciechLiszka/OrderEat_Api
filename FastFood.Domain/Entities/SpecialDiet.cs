namespace FastFood.Domain.Entities
{
    public class SpecialDiet
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}