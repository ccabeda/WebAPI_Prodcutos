using Proyecto_Final.DTOs.ProductoDto;
using Proyecto_Final.Models;

namespace Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryVenta : IRepositoryGeneric<Venta>
    {
        Task<List<Venta>> ObtenerPorIdUsuario(int idUsuario);
    }
}
