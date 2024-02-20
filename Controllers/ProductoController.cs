using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Services.IService;
using System.Net;

namespace WebApi_Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IServiceProducto _service;
        public ProductoController(IServiceProducto service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductos()
        {
            var resultado = await _service.ListarProductos();
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductosByIdUsuario(int idUsuario)
        {
            var resultado = await _service.ListarProductosPorIdUsuario(idUsuario);
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

        [HttpGet(("Id/{id}"), Name = "GetProductobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProducto(int id)
        {
            var resultado = await _service.ObtenerProducto(id);
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
        public async Task<ActionResult<APIResponse>> CreateProducto(ProductoCreateDto productoCreate)
        {
            var resultado = await _service.CrearProducto(productoCreate);
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
        public async Task<ActionResult<APIResponse>> UpdateProducto(ProductoUpdateDto productoUpdate)
        {
            var resultado = await _service.ModificarProducto(productoUpdate);
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

        [HttpDelete("{idProducto}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProducto(int idProducto)
        {
            var resultado = await _service.EliminarProducto(idProducto);
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
