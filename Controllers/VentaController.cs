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
        public async Task<ActionResult<APIResponse>> GetVentasByIdUsuario(int userId)
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

        [HttpGet(("Id/{id}"), Name = "GetVentabyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenta(int id)
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
        public async Task<ActionResult<APIResponse>> CreateVenta(VentaCreateDto saleCreate)
        {
            var result = await _service.Create(saleCreate);
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

        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVentaConIdUsuario(int userId,[FromBody] List<ProductoDtoParaVentas> products)
        {
            var result = await _service.CreateByUserId(userId, products);
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
        public async Task<ActionResult<APIResponse>> UpdateVenta(VentaUpdateDto saleUpdate)
        {
            var result = await _service.Update(saleUpdate);
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
        public async Task<ActionResult<APIResponse>> DeleteVenta(int id)
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
