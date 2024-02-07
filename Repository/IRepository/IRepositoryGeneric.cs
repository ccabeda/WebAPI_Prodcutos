namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryGeneric<T> where T : class
    {
        T ObtenerPorId(int id);
        List<T> ObtenerTodos();
        void Crear(T entity);
        void Actualizar(T entity);
        void Eliminar(T entity);
        void Guardar();
    }
}
