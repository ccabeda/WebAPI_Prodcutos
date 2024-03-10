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
        public APIResponse GetName()
        {
            try
            {
                _apiResponse.Result = "Pearson Specter SA";
                _apiResponse.StatusCode = HttpStatusCode.OK;
                 return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Nombre: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}
