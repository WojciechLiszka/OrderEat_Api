namespace OrderEat.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<User> Users { get; set; } = default!;
    }
}