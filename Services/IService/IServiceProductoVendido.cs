using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProductoVendido
    {
        APIResponse ObtenerProductoVendido(int id);
        APIResponse ListarProductosVendidos();
        APIResponse CrearProductoVendido(ProductoVendidoCreateDto productoVendidoCreate);
        APIResponse ModificarProductoVendido(int id, ProductoVendidoUpdateDto productoVendidoUpdate);
        APIResponse EliminarProductoVendido(int id);
    }
}
