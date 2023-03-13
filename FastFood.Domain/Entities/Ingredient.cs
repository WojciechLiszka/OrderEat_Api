namespace FastFood.Domain.Entities
{
    public class Ingredient
    {
        public int Name { get; set; }
        public string Description { get; set; } = default!;

        public int DishId { get; set; }
        
        public int Prize { get; set; }
        public bool IsRequired { get; set; }
    }
}