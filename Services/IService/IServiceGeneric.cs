using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models.APIResponse;

namespace WebApi_Proyecto_Final.Services.IService
{
    public interface IServiceGeneric<T,A> where T : class
    {
        Task<APIResponse> GetById(int id);
        Task<APIResponse> GetAll();
        Task<APIResponse> Create([FromBody] A entity);
        Task<APIResponse> Update(T entity);
        Task<APIResponse> Delete(int id);
    }
}
