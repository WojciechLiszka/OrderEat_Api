using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Prize { get; set; }

        public bool IsRequired { get; set; }
        public virtual Dish Dish { get; set; } = default!;
        public List<Allergen> Allergens { get; set; } = new List<Allergen>();
    }
}