using SuperMarket.Domain.DTO;

namespace SuperMarket.Services.Interfaces
{
    public interface ITokenService
    {
        Task<ApiResponseModel<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);

        Task<ApiResponseModel<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}
