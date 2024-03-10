using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Services.IService;
using System.Net;

namespace WebApi_Proyecto_Final.Controllers
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
            var result = await _service.GetAll();
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

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductosVendidosByIdUsuario(int userId)
        {
            var result = await _service.GetAllByUserId(userId);
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

        [HttpGet(("Id/{id}"), Name = "GetProductoVendidobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductoVendido(int id)
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateProducto(ProductoVendidoCreateDto productSoldCreate)
        {
            var result = await _service.Create(productSoldCreate);
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
        public async Task<ActionResult<APIResponse>> UpdateProductoVendido(ProductoVendidoUpdateDto productSoldUpdate)
        {
            var result = await _service.Update(productSoldUpdate);
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
        public async Task<ActionResult<APIResponse>> DeleteProductoVendido(int id)
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
