using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; }= default!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrize { get; set; }
        public int BaseCaloricValue { get; set; }

        public bool AllowedCustomization { get; set; }
        public bool IsAvilable { get; set; }

        public int RestaurantId { get; set; }=default!;

        public List<Ingredient> BaseIngreedients { get; set; } = new List<Ingredient>();
        public List<SpecialDiet> AllowedForDiets { get; set; } = new List<SpecialDiet>();
        
    }
}