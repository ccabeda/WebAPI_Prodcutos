using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryVenta : IRepositoryGeneric<Venta>
    {
        Task<List<Venta>> ObtenerPorIdUsuario(int idUsuario);
    }
}
