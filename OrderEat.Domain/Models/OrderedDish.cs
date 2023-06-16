using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Models
{
    public class OrderedDish
    {
        public int DishId { get; set; }
        public string Name { get; set; } = default!;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public decimal prize;
    }
}