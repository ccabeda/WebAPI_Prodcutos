using Proyecto_Final.Models.APIResponse;
using Proyecto_Final.DTOs.VentaDto;

namespace Proyecto_Final.Services.IService
{
    public interface IServiceVenta
    {
        Task<APIResponse> ObtenerVenta(int id);
        Task<APIResponse> ListarVentasPorIdUsuario(int id);
        Task<APIResponse> ListarVentas();
        Task<APIResponse> CrearVenta(VentaCreateDto ventaCreate);
        Task<APIResponse> ModificarVenta(VentaUpdateDto ventaUpdate);
        Task<APIResponse> EliminarVenta(int id);
    }
}
