using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceVenta : IServiceGeneric<VentaUpdateDto, VentaCreateDto>
    {
        Task<APIResponse> GetAllByUserId(int id);
        Task<APIResponse> Create(VentaCreateDto saleCreate);
        Task<APIResponse> CreateByUserId(int idUsuario, List<ProductoDtoParaVentas> productos);
    }
}
