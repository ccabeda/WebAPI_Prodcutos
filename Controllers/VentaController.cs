using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using System.Net;


namespace WebApi_Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IServiceVenta _service;
        public VentaController(IServiceVenta service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVentas()
        {
            var resultado = await _service.ListarVentas();
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

        [HttpGet("{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVentasByIdUsuario(int idUsuario)
        {
            var resultado = await _service.ListarVentasPorIdUsuario(idUsuario);
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

        [HttpGet(("Id/{id}"), Name = "GetVentabyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenta(int id)
        {
            var resultado = await _service.ObtenerVenta(id);
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
        public async Task<ActionResult<APIResponse>> CreateVenta(VentaCreateDto ventaCreate)
        {
            var resultado = await _service.CrearVenta(ventaCreate);
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

        [HttpPost("{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVentaConIdUsuario(int idUsuario,[FromBody] List<ProductoDtoParaVentas> productos)
        {
            var resultado = await _service.CrearVentaConIdUsuario(idUsuario, productos);
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
        public async Task<ActionResult<APIResponse>> UpdateVenta(VentaUpdateDto ventaUpdate)
        {
            var resultado = await _service.ModificarVenta(ventaUpdate);
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
        public async Task<ActionResult<APIResponse>> DeleteVenta(int id)
        {
            var resultado = await _service.EliminarVenta(id);
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
