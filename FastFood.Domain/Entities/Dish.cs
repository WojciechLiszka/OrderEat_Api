namespace FastFood.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int BasePrize { get; set; }
        public int CaloricValue { get; set; }
    }
}