namespace OrderEat.Application.Ingredient.Command
{
    public class IngredientDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Prize { get; set; }
        public bool IsRequired { get; set; }
    }
}