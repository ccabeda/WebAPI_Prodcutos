using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceUsuario
    {
        APIResponse ObtenerUsuario(int id);
        APIResponse ListarUsuarios();
        APIResponse CrearUsuario(UsuarioCreateDto usuarioCreate);
        APIResponse ModificarUsuario(int id, UsuarioUpdateDto usuarioUpdate);
        APIResponse EliminarUsuario(int id);
    }
}
