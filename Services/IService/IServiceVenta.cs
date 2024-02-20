using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceVenta
    {
        Task<APIResponse> ObtenerVenta(int id);
        Task<APIResponse> ListarVentasPorIdUsuario(int id);
        Task<APIResponse> ListarVentas();
        Task<APIResponse> CrearVenta(VentaCreateDto ventaCreate);
        Task<APIResponse> ModificarVenta(VentaUpdateDto ventaUpdate);
        Task<APIResponse> EliminarVenta(int id);
        Task<APIResponse> CrearVentaConIdUsuario(int idUsuario, List<ProductoDtoParaVentas> productos);
    }
}
