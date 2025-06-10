using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly UserService _userService;
        private readonly ReviewService _reviewService;
        public ReviewController(UserService userService, ReviewService reviewService) { 
            _userService = userService;
            _reviewService = reviewService;
        }
        [Authorize]
        [HttpPost("Service")]
        public async Task<IActionResult>  AddReview([FromBody] ReviewDto reviewDto)
        {
            Usuario usuario = await GetAuthorizedUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
            bool existreview = await _reviewService.GetExixtsServiceReview(reviewDto.ServicioId, usuario.Id);
            if (existreview) {
                return Conflict();
            }
            await _reviewService.InsertReview(reviewDto,usuario.Id);
            return Ok();
        }
        [Authorize]
        [HttpPost("Product")]
        public async Task<IActionResult> AddReviewProduct([FromBody] ReviewDto reviewDto)
        {
            Usuario usuario = await GetAuthorizedUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
            bool existreview = await _reviewService.GetExixtsProductReview(reviewDto.ServicioId, usuario.Id);
            if (existreview)
            {
                return Conflict();
            }
            await _reviewService.InsertReviewProduct(reviewDto, usuario.Id);
            return Ok();
        }
        private async Task<Usuario> GetAuthorizedUser()
        {
            // Pilla el usuario autenticado según ASP
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string idString = currentUser.Claims.First().ToString().Substring(3); // 3 porque en las propiedades sale "id: X", y la X sale en la tercera posición

            // Pilla el usuario de la base de datos
            return await _userService.GetUserFromDbByStringId(idString);
        }
    }
}
