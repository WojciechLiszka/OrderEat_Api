using FastFood.Domain.Entities;

namespace FastFood.Domain.Models
{
    public class OrderedDish
    {
        public int DishId { get; set; }
        public string Name { get; set; } = default!;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public decimal prize;
    }
}