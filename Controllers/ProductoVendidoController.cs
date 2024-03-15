using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.Services.Utils;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProductosVendidos()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentar estado de respuesta
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProductosVendidosByIdUsuario(int userId)
        {
            var result = await _service.GetAllByUserId(userId);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("Id/{id}"), Name = "GetProductoVendidobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProductoVendido(int id)
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
        public async Task<ActionResult<APIResponse>> CreateProducto(ProductoVendidoCreateDto productSoldCreate)
        {
            var result = await _service.Create(productSoldCreate);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProductoVendido(ProductoVendidoUpdateDto productSoldUpdate)
        {
            var result = await _service.Update(productSoldUpdate);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteProductoVendido(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }
    }
}
