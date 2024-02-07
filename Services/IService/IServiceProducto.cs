using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceProducto
    {
        APIResponse ObtenerProducto(int id);
        APIResponse ListarProductos();
        APIResponse CrearProducto(ProductoCreateDto productoCreate);
        APIResponse ModificarProducto(int id, ProductoUpdateDto productoUpdate);
        APIResponse EliminarProducto(int id);
    }
}
