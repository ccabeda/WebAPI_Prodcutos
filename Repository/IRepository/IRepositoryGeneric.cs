namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task<T?> ObtenerPorId(int id);
        Task<List<T>> ObtenerTodos();
        Task Crear(T entity);
        Task Actualizar(T entity);
        Task Eliminar(T entity);
        Task Guardar();
    }
}
