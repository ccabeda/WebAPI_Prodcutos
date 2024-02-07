using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
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
        public APIResponse GetUsuarios()
        {
            return _service.ListarUsuarios();
        }

        [HttpGet(("{id}"), Name = "GetUsuariobyId")]
        public APIResponse GetUsuario(int id)
        {
            return _service.ObtenerUsuario(id);
        }

        [HttpPost]
        public APIResponse CreateUsuario(UsuarioCreateDto usuarioCreate)
        {
            return _service.CrearUsuario(usuarioCreate);
        }

        [HttpPut]
        public APIResponse UpdateUsuario(int id, UsuarioUpdateDto usuarioUpdate)
        {
            return _service.ModificarUsuario(id, usuarioUpdate);
        }

        [HttpDelete]
        public APIResponse DeleteUsuario(int id)
        {
            return _service.EliminarUsuario(id);
        }
    }
}
