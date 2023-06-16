namespace OrderEat.Application.Ingredient.Queries
{
    public class GetIngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Prize { get; set; }
        public bool IsRequired { get; set; }
    }
}