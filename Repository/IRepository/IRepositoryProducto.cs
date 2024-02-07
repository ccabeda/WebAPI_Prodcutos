using Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProducto : IRepositoryGeneric<Producto>
    {
        Producto ObtenerPorNombre(string nombre);
    }
}
