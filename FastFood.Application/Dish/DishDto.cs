namespace FastFood.Application.Dish
{
    public class GetDishDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public decimal BasePrize { get; set; } = 0;
        public int BaseCaloricValue { get; set; } = 0;

        public bool AllowedCustomization { get; set; }
        public bool IsAvilable { get; set; }
    }
}