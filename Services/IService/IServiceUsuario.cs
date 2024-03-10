using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceUsuario
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetByUsername(string username);
        Task<APIResponse> Login(string username, string password);
        Task<APIResponse> GetAll();
        Task<APIResponse> Create(UsuarioCreateDto userCreate);
        Task<APIResponse> Update(UsuarioUpdateDto userUpdate);
        Task<APIResponse> Delete(int id);
    }
}
