using Proyecto_Final.Models;

namespace Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryUsuario : IRepositoryGeneric<Usuario>
    {
        Task<Usuario?> ObtenerPorNombre(string nombre);
        Task<Usuario?> ObtenerPorMail(string nombre);
    }
}
