using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.VentaDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceVenta
    {
        APIResponse ObtenerVenta(int id);
        APIResponse ListarVentas();
        APIResponse CrearVenta(VentaCreateDto ventaCreate);
        APIResponse ModificarVenta(int id, VentaUpdateDto ventaUpdate);
        APIResponse EliminarVenta(int id);
    }
}
