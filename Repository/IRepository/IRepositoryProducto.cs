using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProducto : IRepositoryGeneric<Producto>
    {
        Task<Producto?> ObtenerPorNombre(string nombre);
        Task<List<Producto>> ObtenerPorIdUsuario(int idUsuario);
    }
}
