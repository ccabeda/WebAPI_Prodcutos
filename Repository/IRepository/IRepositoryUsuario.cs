using Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryUsuario : IRepositoryGeneric<Usuario>
    {
        Usuario ObtenerPorNombre(string nombre);
        Usuario ObtenerPorMail(string nombre);
    }
}
