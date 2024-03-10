using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceVenta
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetAllByUserId(int id);
        Task<APIResponse> GetAll();
        Task<APIResponse> Create(VentaCreateDto saleCreate);
        Task<APIResponse> Update(VentaUpdateDto saleUpdate);
        Task<APIResponse> Delete(int id);
        Task<APIResponse> CreateByUserId(int idUsuario, List<ProductoDtoParaVentas> productos);
    }
}
