namespace FastFood.Domain.Models
{
    public class AuthenticationSettings
    {
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; } = default!;
        public string JwtKey { get; set; } = default!;
    }
}