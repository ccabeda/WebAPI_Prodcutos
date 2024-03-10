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

        public async Task Update(ProductoVendido productSold)
        {
            _context.Update(productSold);
            await Save();
        }

        public async Task Create(ProductoVendido productSold)
        {
            await _context.ProductoVendidos.AddAsync(productSold);
            await Save();
        }

        public async Task Delete(ProductoVendido productSold)
        {
            _context.ProductoVendidos.Remove(productSold);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ProductoVendido?> GetById(int id)
        {
            return await _context.ProductoVendidos.FindAsync(id);
        }

        public async Task<List<ProductoVendido>> GetAll()
        {
            return await _context.ProductoVendidos.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductoVendido>> GetByProductId(int productId)
        {
            return await _context.ProductoVendidos.Where(p => p.IdProducto == productId).ToListAsync();
        }
    }
}
