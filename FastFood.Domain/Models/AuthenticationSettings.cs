namespace FastFood.Domain.Models
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = default!;
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; } = default!;
    }
}