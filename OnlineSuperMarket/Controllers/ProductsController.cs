using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Domain.DTO;
using SuperMarket.Services.Interfaces;

namespace OnlineSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ApiResponseModel<IReadOnlyList<Product>>> GetAllAsync([FromQuery] ProductFilterRequest request)
        {
            var result = await productService.GetAllAsync(request);

            var totalCount = await productService.GetProductCountAsync();

            return new ApiResponseModel<IReadOnlyList<Product>>(result, totalCount);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponseModel<Product>> GetAsync(int id)
        {
            var result = await productService.GetAsync(id);

            return new ApiResponseModel<Product>(result);
        }


        [HttpPost]
        [Authorize]
        public async Task<ApiResponseModel<Product>> CreateAsync(ProductCreateRequest request)
        {
            var result = await productService.CreateAsync(request);

            return new ApiResponseModel<Product>(result);
        }
    }
}
