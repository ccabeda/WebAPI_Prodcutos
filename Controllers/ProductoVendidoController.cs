using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models.APIResponse;
using System.Net;
using Proyecto_Final.DTOs.ProductoVendidoDto;
using Proyecto_Final.Services.IService;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVendidoController : ControllerBase
    {
        private readonly IServiceProductoVendido _service;
        public ProductoVendidoController(IServiceProductoVendido service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductosVendidos()
        {
            var resultado = await _service.ListarProductosVendidos();
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

        [HttpGet(("{id}"), Name = "GetProductoVendidobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductoVendido(int id)
        {
            var resultado = await _service.ObtenerProductoVendido(id);
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
        public async Task<ActionResult<APIResponse>> CreateProducto(ProductoVendidoCreateDto productoVendidoCreate)
        {
            var resultado = await _service.CrearProductoVendido(productoVendidoCreate);
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
        public async Task<ActionResult<APIResponse>> UpdateProductoVendido(ProductoVendidoUpdateDto productoUpdate)
        {
            var resultado = await _service.ModificarProductoVendido(productoUpdate);
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
        public async Task<ActionResult<APIResponse>> DeleteProductoVendido(int id)
        {
            var resultado = await _service.EliminarProductoVendido(id);
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
