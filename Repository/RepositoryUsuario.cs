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

        public async Task Actualizar(Usuario usuario)
        {
            _context.Update(usuario);
            await Guardar();
        }

        public async Task Crear(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await Guardar();
        }

        public async Task Eliminar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObtenerPorNombre(string nombre)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.NombreUsuario == nombre);
        }

        public async Task<Usuario?> ObtenerPorMail(string mail)
        {
            return await _context.Usuarios.Include(u => u.Venta).FirstOrDefaultAsync(u => u.Mail == mail);
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            return await _context.Usuarios.AsNoTracking().Include(u => u.Venta).ToListAsync();
        }
    }
}
