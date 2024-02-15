using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models.APIResponse;
using System.Net;
using Proyecto_Final.DTOs.UsuarioDto;
using Proyecto_Final.Services.IService;

namespace Proyecto_Final.Controllers
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
                    return Ok(resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("{id}"), Name = "GetUsuariobyId")]
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
                    return Ok(resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("UserName/{username}"), Name = "GetUsuariobyUserName")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioByUserName(string username)
        {
            var resultado = await _service.ObtenerUsuarioPorNombreUsuario(username);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado);
                default:
                    return NotFound(resultado);
            }
        }

        [HttpGet(("{username}/Login/{password}"), Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Login(string username, string password)
        {
            var resultado = await _service.IniciarSesion(username, password);
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(resultado);
                case HttpStatusCode.OK:
                    return Ok(resultado);
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
                    return Ok(resultado);
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
                    return Ok(resultado);
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
                    return Ok(resultado);
                case HttpStatusCode.Conflict:
                    return Conflict(resultado);
                default:
                    return NotFound(resultado);
            }
        }
    }
}
