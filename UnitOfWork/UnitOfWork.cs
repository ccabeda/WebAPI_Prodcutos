using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AplicationDbContext _context;
        public IRepositoryProducto repositoryProducto {  get; }
        public IRepositoryProductoVendido repositoryproductoVendido { get; }
        public IRepositoryUsuario repositoryUsuario { get; }
        public IRepositoryVenta repositoryVenta { get; }
        public UnitOfWork(AplicationDbContext context, IRepositoryProducto _repositoryProducto, IRepositoryProductoVendido _repositoryProductoVendido, IRepositoryUsuario _repositoryUsuario, 
                          IRepositoryVenta _repositoryVenta)
        {
            _context = context;
            repositoryProducto = _repositoryProducto;
            repositoryproductoVendido = _repositoryProductoVendido;
            repositoryUsuario = _repositoryUsuario;
            repositoryVenta = _repositoryVenta;
        }
        public async Task Save() => await _context.SaveChangesAsync();  
    }
}
