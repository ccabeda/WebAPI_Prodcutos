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

        public async Task Update(Venta sale) =>  _context.Update(sale);
        
        public async Task Create(Venta sale) =>  await _context.Venta.AddAsync(sale);
        
        public async Task Delete(Venta sale) => _context.Venta.Remove(sale);
        
        public async Task<Venta?> GetById(int id) => await _context.Venta.Include(v => v.ProductoVendidos).FirstOrDefaultAsync(v => v.Id == id);
        
        public async Task<List<Venta>> GetAll() => await _context.Venta.AsNoTracking().Include(v => v.ProductoVendidos).ToListAsync();
        
        public async Task<List<Venta>> GetAllByUserId(int userId) => await _context.Venta.Include(v => v.ProductoVendidos).Where(v => v.IdUsuario == userId).ToListAsync();
        
    }
}
