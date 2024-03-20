using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.Services.IService;
using AutoMapper;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceNombre : IServiceNombre
    {
        private readonly APIResponse _apiResponse;
        private readonly ILogger<ServiceNombre> _logger;
        private readonly IMapper _mapper;
        public ServiceNombre(APIResponse aPIResponse, ILogger<ServiceNombre> logger, IMapper mapper)
        {
            _apiResponse = aPIResponse;
            _logger = logger;
            _mapper = mapper;

        }
        public APIResponse GetName()
        {
            try
            {
                string a = "Pearson Specter SA";
                return Utils.Utils.OKResponse<string, string>(_mapper,a,_apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}
