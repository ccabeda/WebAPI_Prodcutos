using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Services.Utils;


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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVentas()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVentasByIdUsuario(int userId)
        {
            var result = await _service.GetAllByUserId(userId);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("Id/{id}"), Name = "GetVentabyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVenta(int id)
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVenta(VentaCreateDto saleCreate)
        {
            var result = await _service.Create(saleCreate);
            return Utils.ControllerHelper(result);
        }

        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVentaConIdUsuario(int userId,[FromBody] List<ProductoDtoParaVentas> products)
        {
            var result = await _service.CreateByUserId(userId, products);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateVenta(VentaUpdateDto saleUpdate)
        {
            var result = await _service.Update(saleUpdate);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteVenta(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }
    }
}
