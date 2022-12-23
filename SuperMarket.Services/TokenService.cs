using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperMarket.Domain.DTO;
using SuperMarket.Domain.DTO.Exceptions;
using SuperMarket.Domain.Entities;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuperMarket.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSettings _jwtSettings;

        public TokenService(UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings)
        {
            this.userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponseModel<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
        {
            var user = await userManager.FindByEmailAsync(request.Email.Trim().Normalize());
            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }

            if (!user.IsActive)
            {
                throw new ApiException($"User Not Active. Please contact the administrator.");
            }

            if (!user.EmailConfirmed)
            {
                throw new ApiException($"E-Mail not confirmed.");
            }

            return new ApiResponseModel<TokenResponse>(await GenerateTokensAndUpdateUser(user, ipAddress));
        }

        public async Task<ApiResponseModel<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string? userEmail = userPrincipal.GetEmail();
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user is null)
            {
                throw new ApiException($"Authentication Failed.");
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ApiException($"Invalid Refresh Token.");
            }

            return new ApiResponseModel<TokenResponse>(await GenerateTokensAndUpdateUser(user, ipAddress));
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApiException($"Invalid Token.");
            }

            return principal;
        }

        private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
        {
            var token = GenerateJwt(user, ipAddress);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await userManager.UpdateAsync(user);

            return new TokenResponse(token.Item1, user.RefreshToken, token.Item2, user.RefreshTokenExpiryTime);
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private (string, DateTime) GenerateJwt(ApplicationUser user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

        private (string, DateTime) GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return (tokenHandler.WriteToken(token), token.ValidTo);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
           new List<Claim>
           {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email)
           };
    }
}
