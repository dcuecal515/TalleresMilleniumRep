using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService) {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginDto loginDto)
        {
            Usuario user=await _userService.GetUserByEmailAndPassword(loginDto.email,loginDto.password);
            if(user != null)
            {
                string token = _userService.ObtainToken(user);
                return Ok(new LoginResultDto { accessToken = token });
            }
                return Unauthorized();
        }

        [HttpPost("signup")]
        public async Task<LoginResultDto> RegisterUser([FromForm] SignUpDto signUpDto)
        {
            string token = await _userService.RegisterUser(signUpDto);
            if (token == null)
            {
                return null;
            }
            LoginResultDto loginResultDto = new LoginResultDto { accessToken = token };
            return loginResultDto;
        }

        [HttpGet("emailExists")]
        public async Task<IActionResult> GetIfEmailExists([FromQuery] string correo)
        {
            Boolean returnResult = await _userService.GetIfEmailExists(correo);
            if (returnResult) {
                return Unauthorized();
            } else
            {
                return Ok();
            }
        }
    }
}
