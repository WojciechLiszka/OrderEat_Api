namespace FastFood.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public DateTime DateofBirth { get; set; } = default!;

        public UserContactDetails ContactDetails { get; set; } = default!;
        public List<Role> Roles { get; set; } = default!;
        public SpecialDiet Diet { get; set; } = default!;
    }
}