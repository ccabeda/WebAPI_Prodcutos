using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProducto : IRepositoryGeneric<Producto>
    {
        Task<Producto?> GetByName(string nombre);
        Task<List<Producto>> GetAllByUserId(int userId);
    }
}
