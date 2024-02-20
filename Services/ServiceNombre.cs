using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.Services.IService;
using System.Net;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceNombre : IServiceNombre
    {
        private readonly APIResponse _apiResponse;
        private readonly ILogger<ServiceNombre> _logger;
        public ServiceNombre(APIResponse aPIResponse, ILogger<ServiceNombre> logger)
        {
            _apiResponse = aPIResponse;
            _logger = logger;

        }
        public APIResponse ObtenerNombre()
        {
            try
            {
                _apiResponse.Resultado = "Pearson Specter SA";
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                 return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}
