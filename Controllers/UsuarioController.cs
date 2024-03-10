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
            var result = await _service.GetAll();
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result); //para que funcione el frontend
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("Id/{id}"), Name = "GetUsuariobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuario(int id)
        {
            var result = await _service.GetById(id);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{username}"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioByUserName(string username)
        {
            var result = await _service.GetByUsername(username);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{user}/{password}"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Login(string user, string password)
        {
            var result = await _service.Login(user, password);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateUsuario(UsuarioCreateDto userCreate)
        {
            var result = await _service.Create(userCreate);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                case HttpStatusCode.Conflict:
                    return Conflict(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateUsuario(UsuarioUpdateDto userUpdate)
        {
            var result = await _service.Update(userUpdate);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                case HttpStatusCode.Conflict:
                    return Conflict(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteUsuario(int id)
        {
            var result = await _service.Delete(id);
            switch (result.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                case HttpStatusCode.Conflict:
                    return Conflict(result);
                default:
                    return NotFound(result);
            }
        }
    }
}
