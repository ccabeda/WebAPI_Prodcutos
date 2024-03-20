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

        public  async Task Update(ProductoVendido productSold) => _context.Update(productSold);
        
        public async Task Create(ProductoVendido productSold) => await _context.ProductoVendidos.AddAsync(productSold);
        
        public async Task Delete(ProductoVendido productSold) => _context.ProductoVendidos.Remove(productSold);
        
        public async Task<ProductoVendido?> GetById(int id) => await _context.ProductoVendidos.FindAsync(id);
        
        public async Task<List<ProductoVendido>> GetAll() => await _context.ProductoVendidos.AsNoTracking().ToListAsync();
        
        public async Task<List<ProductoVendido>> GetByProductId(int productId) => await _context.ProductoVendidos.Where(p => p.IdProducto == productId).ToListAsync();
    }
}
