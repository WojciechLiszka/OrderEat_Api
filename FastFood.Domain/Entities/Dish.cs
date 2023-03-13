namespace FastFood.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public int BasePrize { get; set; }
        public int BaseCaloricValue { get; set; }

        public bool AllowedCustomization { get; set; }
        public bool IsAvilable { get; set; }

        public List<Ingredient> BaseIngreedient { get; set; } = new List<Ingredient>();
        public List<SpecialDiet> AllowedDiets { get; set; } = new List<SpecialDiet>();
        public List<Allergen> Allergens { get; set; } = new List<Allergen>();
    }
}