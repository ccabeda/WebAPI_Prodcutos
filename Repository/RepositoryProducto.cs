using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models;
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

        public async Task Update(Producto product) => _context.Update(product);

        public async Task Create(Producto product) => await _context.Productos.AddAsync(product);
        
        public async Task Delete(Producto product) => _context.Productos.Remove(product);

        public async Task<Producto?> GetById(int id) => await _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Producto?> GetByName(string name) => await _context.Productos.Include(p => p.ProductoVendidos).FirstOrDefaultAsync(p => p.Descripciones == name);
        
        public async Task<List<Producto>> GetAll() => await _context.Productos.AsNoTracking().Include(p => p.ProductoVendidos).ToListAsync();

        public async Task<List<Producto>> GetAllByUserId(int userId) => await _context.Productos.Include(v => v.ProductoVendidos).Where(v => v.IdUsuario == userId).ToListAsync();
        
    }
}
