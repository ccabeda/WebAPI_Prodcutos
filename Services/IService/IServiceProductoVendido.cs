using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProductoVendido
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetAll();
        Task<APIResponse> GetAllByUserId(int userId);
        Task<APIResponse> Create(ProductoVendidoCreateDto productSoldCreate);
        Task<APIResponse> Update(ProductoVendidoUpdateDto productSoldUpdate);
        Task<APIResponse> Delete(int id);
    }
}
