using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ProductFullDto> GetAllService([FromQuery] QueryDto queryDto)
        {
            ProductFullDto productos = await _productService.GetallProduct(queryDto);
            return productos;
        }
        [HttpGet("{id}")]
        public async Task<ProductoDto> GetProductById(int id)
        {
            ProductoDto productoDto = await _productService.GetProductById(id);
            return productoDto;
        }
    }
}
