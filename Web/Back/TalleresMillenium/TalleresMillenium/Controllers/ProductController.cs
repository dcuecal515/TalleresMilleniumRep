using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;
        public ProductController(ProductService productService, UserService userService)
        {
            _productService = productService;
            _userService = userService;
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
        [Authorize]
        [HttpGet("full")]
        public async Task<ICollection<ProductAdminDto>> GetAllFullProduct()
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return null;
            }
            ICollection<ProductAdminDto> productAdminDto=await _productService.GetAllProductFull();
            return productAdminDto;
        }
        private async Task<Usuario> GetCurrentUser()
        {
            // Pilla el usuario autenticado según ASP
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string idString = currentUser.Claims.First().ToString().Substring(3); // 3 porque en las propiedades sale "id: X", y la X sale en la tercera posición

            // Pilla el usuario de la base de datos
            return await _userService.GetUserFromDbByStringId(idString);
        }
    }
}
