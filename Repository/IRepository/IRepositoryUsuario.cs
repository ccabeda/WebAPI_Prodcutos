using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryUsuario : IRepositoryGeneric<Usuario>
    {
        Task<Usuario?> GetByName(string name);
        Task<Usuario?> GetByMail(string mail);
    }
}
