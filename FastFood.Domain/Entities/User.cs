namespace FastFood.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = default!;
        public string? PasswordHash { get; set; }
        public DateTime? DateofBirth { get; set; }

        public UserContactDetails ContactDetails { get; set; }
        public Role? Role { get; set; }
        public SpecialDiet? Diet { get; set; }
    }
}