using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceUsuario
    {
        Task<APIResponse> ObtenerUsuario(int id);
        Task<APIResponse> ObtenerUsuarioPorNombreUsuario(string username);
        Task<APIResponse> IniciarSesion(string username, string password);
        Task<APIResponse> ListarUsuarios();
        Task<APIResponse> CrearUsuario(UsuarioCreateDto usuarioCreate);
        Task<APIResponse> ModificarUsuario(UsuarioUpdateDto usuarioUpdate);
        Task<APIResponse> EliminarUsuario(int id);
    }
}
