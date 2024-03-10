using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.Services.IService;
using System.Net;

namespace WebApi_Proyecto_Final.Controllers
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
        public ActionResult<APIResponse> GetName()
        {
           var result = _service.GetName();
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result.Result);
                default:
                    return NotFound(result);
            }
        }
    }
}
