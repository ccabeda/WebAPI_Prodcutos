using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Services.IService;

namespace WebApi_Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario _service;
        public UsuarioController(IServiceUsuario service)
        {
            _service = service;
        }

        [HttpGet] //prueba de un endpoint get. Aun no deben hacerse
        public async Task<APIResponse> GetUsuarios()
        {
            return await _service.ListarUsuarios();
        }

        [HttpGet(("{id}"), Name = "GetUsuariobyId")]
        public async Task<APIResponse> GetUsuario(int id)
        {
            return await _service.ObtenerUsuario(id);
        }

        [HttpPost]
        public async Task<APIResponse> CreateUsuario(UsuarioCreateDto usuarioCreate)
        {
            return await _service.CrearUsuario(usuarioCreate);
        }

        [HttpPut]
        public async Task<APIResponse> UpdateUsuario(int id, UsuarioUpdateDto usuarioUpdate)
        {
            return await _service.ModificarUsuario(id, usuarioUpdate);
        }

        [HttpDelete]
        public async Task<APIResponse> DeleteUsuario(int id)
        {
            return await _service.EliminarUsuario(id);
        }
    }
}
