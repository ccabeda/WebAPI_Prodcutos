using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProducto
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetAllByUserId(int id); 
        Task<APIResponse> GetAll();
        Task<APIResponse> Create(ProductoCreateDto productCreate);
        Task<APIResponse> Update(ProductoUpdateDto productUpdate);
        Task<APIResponse> Delete(int id);
    }
}
