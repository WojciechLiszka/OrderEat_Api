using FastFood.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Fee { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public List<OrderedDish> OrderedDishes { get; set; } = new List<OrderedDish>(); 
    }
}