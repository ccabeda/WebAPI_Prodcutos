using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProductoVendido : IServiceGeneric<ProductoVendidoUpdateDto, ProductoVendidoCreateDto>
    {
        Task<APIResponse> GetAllByUserId(int userId);
    }
}
