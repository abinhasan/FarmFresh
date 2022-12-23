using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Domain.DTO;
using SuperMarket.Services.Interfaces;

namespace OnlineSuperMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ApiResponseModel<IReadOnlyList<Category>>> GetAllAsync()
        {
            var result = await categoryService.GetAllAsync();

            return new ApiResponseModel<IReadOnlyList<Category>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponseModel<Category>> GetAsync(int id)
        {
            var result = await categoryService.GetAsync(id);

            return new ApiResponseModel<Category>(result);
        }

        [HttpPost]
        public async Task<ApiResponseModel<Category>> CreateAsync(CategoryCreateRequest request)
        {
            var result = await categoryService.CreateAsync(request);

            return new ApiResponseModel<Category>(result);
        }
    }
}
