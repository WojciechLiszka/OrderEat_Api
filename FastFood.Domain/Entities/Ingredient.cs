namespace FastFood.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Description { get; set; } = default!;
        public int Prize { get; set; }
        public bool IsRequired { get; set; }
        public Dish Dish { get; set; } = default!;
        public List<Allergen> Allergens { get; set; } = new List<Allergen>();
    }
}