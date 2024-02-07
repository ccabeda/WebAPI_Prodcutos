using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceVenta
    {
        Task<APIResponse> ObtenerVenta(int id);
        Task<APIResponse> ListarVentas();
        Task<APIResponse> CrearVenta(VentaCreateDto ventaCreate);
        Task<APIResponse> ModificarVenta(int id, VentaUpdateDto ventaUpdate);
        Task<APIResponse> EliminarVenta(int id);
    }
}
