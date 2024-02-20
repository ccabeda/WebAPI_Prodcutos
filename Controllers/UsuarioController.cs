using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Services.IService;
using System.Net;

namespace WebApi_Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario _service;
        public UsuarioController(IServiceUsuario service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarios()
        {
            var resultado = await _service.ListarUsuarios();
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado); //para que funcione el frontend
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("Id/{id}"), Name = "GetUsuariobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuario(int id)
        {
            var resultado = await _service.ObtenerUsuario(id);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("{nombreDeUsuario}"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioByUserName(string nombreDeUsuario)
        {
            var resultado = await _service.ObtenerUsuarioPorNombreUsuario(nombreDeUsuario);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("{usuario}/{password}"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Login(string usuario, string password)
        {
            var resultado = await _service.IniciarSesion(usuario, password);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateUsuario(UsuarioCreateDto usuarioCreate)
        {
            var resultado = await _service.CrearUsuario(usuarioCreate);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                case HttpStatusCode.Conflict:
                    return Conflict(resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateUsuario(UsuarioUpdateDto usuarioUpdate)
        {
            var resultado = await _service.ModificarUsuario(usuarioUpdate);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                case HttpStatusCode.Conflict:
                    return Conflict(resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteUsuario(int id)
        {
            var resultado = await _service.EliminarUsuario(id);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado.Resultado);
                case HttpStatusCode.Conflict:
                    return Conflict(resultado);
                default:
                    return NotFound(resultado);
            }
        }
    }
}
