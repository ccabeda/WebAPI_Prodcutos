using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProductoVendido
    {
        Task<APIResponse> ObtenerProductoVendido(int id);
        Task<APIResponse> ListarProductosVendidos();
        Task<APIResponse> ListarProductosVendidosPorIdUsuario(int idUsuario);
        Task<APIResponse> CrearProductoVendido(ProductoVendidoCreateDto productoVendidoCreate);
        Task<APIResponse> ModificarProductoVendido(ProductoVendidoUpdateDto productoVendidoUpdate);
        Task<APIResponse> EliminarProductoVendido(int id);
    }
}
