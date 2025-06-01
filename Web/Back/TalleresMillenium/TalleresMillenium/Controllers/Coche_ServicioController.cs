using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Coche_ServicioController : Controller
    {
        private readonly Coche_ServicioService _coche_ServicioService;
        private readonly CocheService _cocheService;
        private readonly UserService _userService;

        public Coche_ServicioController(Coche_ServicioService coche_ServicioService, CocheService cocheService, UserService userService) {
            _coche_ServicioService = coche_ServicioService;
            _cocheService = cocheService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("cochesR")]
        public async Task<List<CocheRDto>> GetCocheRDtos()
        {
            Usuario user = await GetCurrentUser();
            Usuario fullUser = await _userService.GetFullUserById(user.Id);

            if (fullUser.Coches.Count > 0)
            {
                List<CocheRDto> cocheRDtos = [];
                foreach (var coche in fullUser.Coches)
                {
                    CocheRDto cocheRDto = new CocheRDto
                    {
                        Tipo = coche.Tipo,
                        Matricula = coche.Matricula
                    };
                    cocheRDtos.Add(cocheRDto);
                }
                return cocheRDtos;
            } else
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost("reserva")]
        public async Task<IActionResult> ReservarServicio([FromBody] ReservarDto reservarDto)
        {
            Coche coche = await _cocheService.GetCocheByMatricula(reservarDto.Matricula);

            if (coche == null)
            {
                return Unauthorized();
            } else
            {
                bool exists = _coche_ServicioService.GetIfExistsCoche_Sevicio(coche.Id, reservarDto.ServicioId);

                if (exists)
                {
                    return Conflict();
                } else
                {
                    Coche_Servicio coche_Servicio = new Coche_Servicio
                    {
                        CocheId = coche.Id,
                        Estado = "Espera",
                        ServicioId = reservarDto.ServicioId
                    };
                    await _coche_ServicioService.InsertCoche_Servicio(coche_Servicio);
                    return Ok();
                }
            }
        }

        [Authorize]
        [HttpGet("carrito")]
        public async Task<List<ElementoCarritoDto>> GetServiciosEnEspera()
        {
            Usuario user = await GetCurrentUser();
            Usuario fullUser = await _userService.GetFullUserById(user.Id);

            if (fullUser != null && fullUser.Coches.Count > 0)
            {
                List<ElementoCarritoDto> elementoCarritoDtos = new List<ElementoCarritoDto>();
                foreach (var coche in fullUser.Coches)
                {
                    List<ServicioCarritoDto> servicios = new List<ServicioCarritoDto>();
                    if (coche.coche_Servicios.Count > 0)
                    {
                        bool serviciosEnEspera = false;
                        foreach (var coche_servicio in coche.coche_Servicios)
                        {
                            if(coche_servicio.Estado == "Espera")
                            {
                                ServicioCarritoDto servicio = new ServicioCarritoDto
                                {
                                    Nombre = coche_servicio.servicio.Nombre,
                                    Imagen = coche_servicio.servicio.Imagen
                                };
                                servicios.Add(servicio);
                                serviciosEnEspera = true;
                            }
                        }
                        if (serviciosEnEspera)
                        {
                            ElementoCarritoDto element = new ElementoCarritoDto
                            {
                                Tipo = coche.Tipo,
                                Matricula = coche.Matricula,
                                Servicios = servicios
                            };
                            elementoCarritoDtos.Add(element);
                        }
                    }
                }
                return elementoCarritoDtos;
            } else
            {
                return null;
            }
        }

        [Authorize]
        [HttpDelete("eliminarServicio")]
        public async Task<IActionResult> EliminarServicioCarrito([FromQuery] Coche_ServicioEliminarDto coche_Servicio)
        {
            Coche_Servicio coche_Servicio1 = _coche_ServicioService.GetCoche_ServicioByMYN(coche_Servicio.Matricula, coche_Servicio.NombreServicio);
            if (coche_Servicio1 == null) {
                return Unauthorized();
            } else
            {
                await _coche_ServicioService.DeleteCoche_Servicio(coche_Servicio1);
                return Ok();
            }
        }

        [Authorize]
        [HttpPost("completarReserva")]
        public async Task<IActionResult> CompletarReserva([FromBody] MatriculaDto matriculaDto)
        {
            Coche coche = await _cocheService.GetCocheByMatricula(matriculaDto.Matricula);

            if (coche == null)
            {
                return Unauthorized();
            } else
            {
                if (coche.coche_Servicios.Count == 0)
                {
                    return Conflict();
                } else
                {
                    foreach (var coche_Servicio in coche.coche_Servicios)
                    {
                        coche_Servicio.Estado = "Reservado";
                        await _coche_ServicioService.UpdateCoche_Servicio(coche_Servicio);
                    }
                    return Ok();
                }
            }
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
