namespace WebApi_Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<List<T>> GetAll();
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task Save();
    }
}
