using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryVenta : IRepositoryVenta
    {
        private readonly AplicationDbContext _context;
        public RepositoryVenta(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Actualizar(Venta venta)
        {
            _context.Update(venta);
            await Guardar();
        }

        public async Task Crear(Venta venta)
        {
            await _context.Venta.AddAsync(venta);
            await Guardar();
        }

        public async Task Eliminar(Venta venta)
        {
            _context.Venta.Remove(venta);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Venta?> ObtenerPorId(int id)
        {
            return await _context.Venta.Include(v => v.ProductoVendidos).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Venta>> ObtenerTodos()
        {
            return await _context.Venta.AsNoTracking().Include(v => v.ProductoVendidos).ToListAsync();
        }

        public async Task<List<Venta>> ObtenerPorIdUsuario(int idUsuario)
        {
            return await _context.Venta.Include(v => v.ProductoVendidos).Where(v => v.IdUsuario == idUsuario).ToListAsync();
        }
    }
}
