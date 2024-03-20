using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly AplicationDbContext _context;
        public RepositoryUsuario(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(Usuario user)
        {
            _context.Update(user);
        }

        public async Task Create(Usuario user)
        {
            await _context.Usuarios.AddAsync(user);
        }

        public async Task Delete(Usuario user)
        {
            _context.Usuarios.Remove(user);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByName(string name)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.NombreUsuario == name);
        }

        public async Task<Usuario?> GetByMail(string mail)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.Mail == mail);
        }

        public async Task<List<Usuario>> GetAll()
        {
            return await _context.Usuarios.AsNoTracking().Include(u => u.Venta).ToListAsync();
        }
    }
}
