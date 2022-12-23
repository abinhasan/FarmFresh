namespace SuperMarket.Domain.DTO
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TokenExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; }
    }
}
