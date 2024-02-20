using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProductoVendido : IRepositoryGeneric<ProductoVendido>
    {
        Task<List<ProductoVendido>> ObtenerPorIdProducto(int idProducto);
    }
}
