namespace SuperMarket.Domain.DTO
{
    public class TokenResponse
    {
        public TokenResponse(string token, string refreshToken, DateTime tokenExpiryTime, DateTime refreshTokenExpiryTime)
        {
            Token = token;
            RefreshToken = refreshToken;
            TokenExpiryTime = tokenExpiryTime;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime TokenExpiryTime { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
