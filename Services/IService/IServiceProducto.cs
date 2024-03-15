using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProducto : IServiceGeneric<ProductoUpdateDto, ProductoCreateDto>
    {
        Task<APIResponse> GetAllByUserId(int id); 
    }
}
