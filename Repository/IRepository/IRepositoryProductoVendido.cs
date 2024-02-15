using Proyecto_Final.Models;

namespace Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProductoVendido : IRepositoryGeneric<ProductoVendido>
    {
        Task<List<ProductoVendido>> ObtenerPorIdProducto(int idProducto);
    }
}
