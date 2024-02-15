using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Database;
using Proyecto_Final.Models;
using Proyecto_Final.Repository.IRepository;

namespace Proyecto_Final.Repository
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly AplicationDbContext _context;
        public RepositoryProducto(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Actualizar(Producto producto)
        {
            _context.Update(producto);
            await Guardar();
        }

        public async Task Crear(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await Guardar();
        }

        public async Task Eliminar(Producto producto)
        {
            _context.Productos.Remove(producto);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto?> ObtenerPorNombre(string nombre)
        {
            return await _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefaultAsync(p => p.Descripciones == nombre);
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Productos.Include(p => p.ProductoVendidos).ToListAsync();
        }
    }
}
