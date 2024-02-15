using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models.APIResponse;
using Proyecto_Final.Services.IService;
using System.Net;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NombreController : ControllerBase
    {
        private readonly IServiceNombre _service;
        public NombreController(IServiceNombre service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse> GetNombre()
        {
           var resultado = _service.ObtenerNombre();
            switch (resultado.EstadoRespuesta)
            {
                case HttpStatusCode.OK:
                    return Ok(resultado);
                default:
                    return NotFound(resultado);
            }
        }
    }
}
