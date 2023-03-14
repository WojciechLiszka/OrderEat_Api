namespace FastFood.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; }

        public int BasePrize { get; set; }
        public int BaseCaloricValue { get; set; }

        public bool AllowedCustomization { get; set; }
        public bool IsAvilable { get; set; }

        public Restaurant Restaurant { get; set; }=default!;

        public List<Ingredient> BaseIngreedients { get; set; } = new List<Ingredient>();
        public List<SpecialDiet> AllowedForDiets { get; set; } = new List<SpecialDiet>();
        
    }
}