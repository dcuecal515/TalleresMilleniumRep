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
        [Authorize]
        [HttpDelete]
        public async Task DeleteProduct([FromQuery] int id)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return;
            }

            Producto deletedProduct = await _productService.getProductByIdOnlyAsync(id);

            if (deletedProduct == null)
            {
                return;
            }

            await _productService.DeleteProduct(deletedProduct);
        }
        [Authorize]
        [HttpPut("change")]
        public async Task<IActionResult> PutProduct([FromForm] ChangeProductDto changeProductDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            bool existproductname = await _productService.GetExixtsProductName(changeProductDto.Nombre);
            if (existproductname)
            {
                return Conflict();
            }
            Producto producto = await _productService.getProductByIdOnlyAsync(changeProductDto.Id);

            if (producto == null)
            {
                return BadRequest();
            }
            producto.Nombre = changeProductDto.Nombre;
            producto.Disponible= changeProductDto.Disponible;
            producto.Descripcion = changeProductDto.Descripcion;
            if (changeProductDto.Imagen != null)
            {
                ImageService imageService = new ImageService();
                producto.Imagen = "/" + await imageService.InsertAsync(changeProductDto.Imagen);
            }

            await _productService.UpdateProduct(producto);
            return Ok();
        }
        [Authorize]
        [HttpPost("new")]
        public async Task<IActionResult> AddProduct([FromForm] NewProductDto newProductDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            bool existproductname = await _productService.GetExixtsProductName(newProductDto.Nombre);
            if (existproductname)
            {
                return Conflict();
            }
            Producto producto = new Producto();
            producto.Nombre= newProductDto.Nombre;
            producto.Descripcion= newProductDto.Descripcion;
            producto.Disponible=newProductDto.Disponible;
            ImageService imageService = new ImageService();
            producto.Imagen = "/" + await imageService.InsertAsync(newProductDto.Imagen);

            await _productService.InsertProduct(producto);
            return Ok();
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
