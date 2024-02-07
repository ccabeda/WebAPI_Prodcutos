using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProducto
    {
        Task<APIResponse> ObtenerProducto(int id);
        Task<APIResponse> ListarProductos();
        Task<APIResponse> CrearProducto(ProductoCreateDto productoCreate);
        Task<APIResponse> ModificarProducto(int id, ProductoUpdateDto productoUpdate);
        Task<APIResponse> EliminarProducto(int id);
    }
}
