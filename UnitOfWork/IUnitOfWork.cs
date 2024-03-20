using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepositoryProducto repositoryProducto { get; }
        public IRepositoryProductoVendido repositoryproductoVendido { get; }
        public IRepositoryUsuario repositoryUsuario { get; }
        public IRepositoryVenta repositoryVenta { get; }
        Task Save();
    }
}
