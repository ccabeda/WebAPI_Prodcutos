using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryProductoVendido : IRepositoryProductoVendido
    {
        private readonly AplicationDbContext _context;
        public RepositoryProductoVendido(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Actualizar(ProductoVendido productoVendido)
        {
            _context.Update(productoVendido);
            await Guardar();
        }

        public async Task Crear(ProductoVendido productoVendido)
        {
            await _context.ProductoVendidos.AddAsync(productoVendido);
            await Guardar();
        }

        public async Task Eliminar(ProductoVendido productoVendido)
        {
            _context.ProductoVendidos.Remove(productoVendido);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ProductoVendido?> ObtenerPorId(int id)
        {
            return await _context.ProductoVendidos.FindAsync(id);
        }

        public async Task<List<ProductoVendido>> ObtenerTodos()
        {
            return await _context.ProductoVendidos.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductoVendido>> ObtenerPorIdProducto(int idProducto)
        {
            return await _context.ProductoVendidos.Where(p => p.IdProducto == idProducto).ToListAsync();
        }
    }
}
