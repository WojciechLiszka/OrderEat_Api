namespace OrderEat.Domain.Entities
{
    public class Allergen
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}