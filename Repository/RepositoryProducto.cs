using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Database;
using Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly AplicationDbContext _context;
        public RepositoryProducto(AplicationDbContext context)
        {
            _context = context;
        }

        public void Actualizar(Producto producto)
        {
            _context.Update(producto);
            Guardar();
        }

        public void Crear(Producto producto)
        {
            _context.Productos.Add(producto);
            Guardar();
        }

        public void Eliminar(Producto producto)
        {
            _context.Productos.Remove(producto);
            _context.SaveChanges();
        }

        public void Guardar()
        {
            _context.SaveChanges();
        }

        public Producto ObtenerPorId(int id)
        {
            var producto = _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefault(p => p.Id == id);
            return producto;
        }

        public Producto ObtenerPorNombre(string nombre)
        {
            var producto = _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefault(p => p.Descripciones == nombre);
            return producto;
        }

        public List<Producto> ObtenerTodos()
        {
            var lista_productos = _context.Productos.Include(p => p.ProductoVendidos).ToList();
            return lista_productos;
        }
    }
}
