namespace FastFood.Domain.Entities
{
    public class SpecialDiet
    {
        public int id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}