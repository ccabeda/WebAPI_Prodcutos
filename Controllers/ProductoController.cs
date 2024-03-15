using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.Services.Utils;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProductos()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProductosByIdUsuario(int userId)
        {
            var result = await _service.GetAllByUserId(userId);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("Id/{id}"), Name = "GetProductobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetProducto(int id)
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
        public async Task<ActionResult<APIResponse>> CreateProducto(ProductoCreateDto productCreate)
        {
            var result = await _service.Create(productCreate);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProducto(ProductoUpdateDto productUpdate)
        {
            var result = await _service.Update(productUpdate);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteProducto(int productId)
        {
            var result = await _service.Delete(productId);
            return Utils.ControllerHelper(result);
        }
    }
}
