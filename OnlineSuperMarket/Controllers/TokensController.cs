using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Domain.DTO;
using SuperMarket.Services;
using SuperMarket.Services.Interfaces;

namespace OnlineSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public TokensController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<ApiResponseModel<TokenResponse>> GetTokenAsync(TokenRequest request)
        {
            return tokenService.GetTokenAsync(request, GetIpAddress());
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public Task<ApiResponseModel<TokenResponse>> RefreshAsync(RefreshTokenRequest request)
        {
            return tokenService.RefreshTokenAsync(request, GetIpAddress());
        }

        private string GetIpAddress() =>
        Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"]
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}
