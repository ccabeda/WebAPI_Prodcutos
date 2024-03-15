using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceUsuario : IServiceGeneric<UsuarioUpdateDto, UsuarioCreateDto>
    {
        Task<APIResponse> GetByUsername(string username);
        Task<APIResponse> Login(string username, string password);
    }
}
