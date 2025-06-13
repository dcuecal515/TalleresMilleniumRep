using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
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
        private readonly EmailService _emailService;

        public Coche_ServicioController(Coche_ServicioService coche_ServicioService, CocheService cocheService, UserService userService,EmailService emailService) {
            _coche_ServicioService = coche_ServicioService;
            _cocheService = cocheService;
            _userService = userService;
            _emailService = emailService;
        }

        [Authorize]
        [HttpGet("cochesR")]
        public async Task<List<CocheRDto>> GetCocheRDtos()
        {
            Usuario user = await GetCurrentUser();
            if (user == null)
            {
                return null;
            }
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
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
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
                        Fecha = DateOnly.FromDateTime(DateTime.Now),
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
            if (user == null)
            {
                return null;
            }
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
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
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
            Usuario usuario = await GetCurrentUser();
            if (usuario == null) {
                return Unauthorized();
            }
            Coche coche = await _cocheService.GetByMatriculaWithoutServiceAsync(matriculaDto.Matricula);
            Coche coche2 = await _cocheService.GetCocheByMatricula(matriculaDto.Matricula);

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
                    List<Coche_Servicio> coche_Servicios = new List<Coche_Servicio>();
                    List<Coche_Servicio> coche_Servicios1 = new List<Coche_Servicio>();
                    foreach (var coche_Servicio in coche2.coche_Servicios)
                    {
                        if (coche_Servicio.Estado == "Espera")
                        {
                            coche_Servicios.Add(coche_Servicio);
                        }
                    }
                    foreach (var coche_Servicio in coche.coche_Servicios)
                    {
                        if (coche_Servicio.Estado=="Espera")
                        {
                            coche_Servicio.Estado = "Reservado";
                            coche_Servicio.Fecha = DateOnly.FromDateTime(DateTime.Now);
                            coche_Servicios1.Add(coche_Servicio);
                        }
                    }
                    await _coche_ServicioService.UpdatemanyCoche_Servicio(coche_Servicios1);
                    await _emailService.CreateEmailUser(usuario.Email,coche_Servicios, matriculaDto.Matricula);

                    return Ok();
                }
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ICollection<Coche_ServicioFullDto>> getallCoche_servicio()
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return null;
            }
            ICollection<Coche_Servicio> coche_Servicios = await _coche_ServicioService.getallCoche_servicio();
            var coche_ServicioFullDtos = new List<Coche_ServicioFullDto>();

            foreach (var coche_servicio in coche_Servicios)
            {
                
                var existe = coche_ServicioFullDtos.FirstOrDefault(r =>r.Matricula == coche_servicio.coche.Matricula && r.Fecha == coche_servicio.Fecha);

                if (existe != null)
                {
                   
                    existe.Servicios.Add(new ServicioCocheDto
                    {
                        Idcoche_servicio = coche_servicio.Id,
                        Nombre = coche_servicio.servicio.Nombre
                    });
                }
                else
                {
                    var coche_ServicioFullDto = new Coche_ServicioFullDto
                    {
                        Estado = coche_servicio.Estado,
                        Fecha = coche_servicio.Fecha,
                        Matricula = coche_servicio.coche.Matricula,
                        Tipo = coche_servicio.coche.Tipo,
                        Servicios = new List<ServicioCocheDto>
                {
                    new ServicioCocheDto
                    {
                         Idcoche_servicio = coche_servicio.Id,
                        Nombre = coche_servicio.servicio.Nombre
                    }
                }
                    };
                    coche_ServicioFullDtos.Add(coche_ServicioFullDto);
                }
            }

            return coche_ServicioFullDtos;
        }
        [Authorize]
        [HttpPut("aceptar")]
        public async Task<IActionResult> AceptarSolicitud([FromBody] Coche_ServicioReservaDto coche_ServicioReserva)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            ICollection<Coche_Servicio> coche_Servicio = await _coche_ServicioService.getAllCoche_ServicioByMatriculaFecha(coche_ServicioReserva.fechaantigua,coche_ServicioReserva.matricula);
            Coche cocheusuario = await _cocheService.GetCocheByMatriculaForEmail(coche_ServicioReserva.matricula);
            List<Coche_Servicio> coche_Servicios = new List<Coche_Servicio>();
            foreach (var cocheservicio in coche_Servicio)
            {
                cocheservicio.Estado = "Aceptado";
                cocheservicio.Fecha = DateOnly.Parse(coche_ServicioReserva.fechanueva);
                coche_Servicios.Add(cocheservicio);
            }
            await _emailService.CreateEmailAceptarUser(cocheusuario.Usuario.Email, coche_Servicios,coche_ServicioReserva.fechanueva, coche_ServicioReserva.matricula);
            await _coche_ServicioService.UpdatemanyCoche_Servicio(coche_Servicios);
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Eliminarsolicitud([FromQuery] int id)
        {

            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            Coche_Servicio deletecoche_Servicio = await _coche_ServicioService.GetCoche_ServicioById(id);
            Coche cocheusuario = await _cocheService.GetCocheByMatriculaForEmail(deletecoche_Servicio.coche.Matricula);
            if (deletecoche_Servicio == null)
            {
                return BadRequest();
            }
            await _emailService.CreateDeleteEmail(cocheusuario.Usuario.Email, deletecoche_Servicio.servicio,deletecoche_Servicio.coche.Matricula);
            await _coche_ServicioService.DeleteCoche_Servicio(deletecoche_Servicio);
            return Ok();
        }
        [Authorize]
        [HttpPut("finalizar")]
        public async Task<IActionResult> Finalizarsolicitud([FromBody] Coche_ServicioFinalizarDto coche_ServicioFinalizarDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            ICollection<Coche_Servicio> coche_Servicios = await _coche_ServicioService.getAllCoche_ServicioByMatriculaFechaFinalizar(coche_ServicioFinalizarDto.matricula, coche_ServicioFinalizarDto.fechaantigua);
            Coche cocheusuario = await _cocheService.GetCocheByMatriculaForEmail(coche_ServicioFinalizarDto.matricula);
            List<Coche_Servicio> coche_Servicioslista = new List<Coche_Servicio>();
            foreach (var cocheservicio in coche_Servicios)
            {
                cocheservicio.Estado = "Finalizado";
                cocheservicio.Fecha = DateOnly.FromDateTime(DateTime.Now);
                coche_Servicioslista.Add(cocheservicio);
            }
            await _emailService.CreateEmailFinalizarUser(cocheusuario.Usuario.Email,coche_Servicioslista,coche_ServicioFinalizarDto.matricula);
            await _coche_ServicioService.UpdatemanyCoche_Servicio(coche_Servicioslista);
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
