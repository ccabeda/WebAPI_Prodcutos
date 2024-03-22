using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceUsuario
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetAll();
        Task<APIResponse> Create([FromBody] UsuarioCreateDto user);
        Task<APIResponse> Update(UsuarioUpdateDto user, string username, string pasword);
        Task<APIResponse> Delete(string username, string password);
        Task<APIResponse> GetByUsername(string username);
        Task<APIResponse> Login(string username, string password);
    }
}
