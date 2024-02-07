using Proyecto_Final.Database;
using Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryProductoVendido : IRepositoryGeneric<ProductoVendido>
    {
        private readonly AplicationDbContext _context;
        public RepositoryProductoVendido(AplicationDbContext context)
        {
            _context = context;
        }

        public void Actualizar(ProductoVendido productoVendido)
        {
            _context.Update(productoVendido);
            Guardar();
        }

        public void Crear(ProductoVendido productoVendido)
        {
            _context.ProductoVendidos.Add(productoVendido);
            Guardar();
        }

        public void Eliminar(ProductoVendido productoVendido)
        {
            _context.ProductoVendidos.Remove(productoVendido);
            _context.SaveChanges();
        }

        public void Guardar()
        {
            _context.SaveChanges();
        }

        public ProductoVendido ObtenerPorId(int id)
        {
            var productoVendido = _context.ProductoVendidos.Find(id);
            return productoVendido;
        }

        public List<ProductoVendido> ObtenerTodos()
        {
            var lista_productosVendidos = _context.ProductoVendidos.ToList();
            return lista_productosVendidos;
        }
    }
}
