using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Database;
using Proyecto_Final.Models;
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

        public void Actualizar(Usuario usuario)
        {
            _context.Update(usuario);
            Guardar();
        }

        public void Crear(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            Guardar();
        }

        public void Eliminar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public void Guardar()
        {
            _context.SaveChanges();
        }

        public Usuario ObtenerPorId(int id)
        {
            var usuario = _context.Usuarios.Include(u => u.Venta).FirstOrDefault(u => u.Id == id);
            return usuario;
        }

        public Usuario ObtenerPorNombre(string nombre)
        {
            var usuario = _context.Usuarios.Include(u => u.Venta).FirstOrDefault(u => u.NombreUsuario == nombre);
            return usuario;
        }

        public Usuario ObtenerPorMail(string mail)
        {
            var usuario = _context.Usuarios.Include(u => u.Venta).FirstOrDefault(u => u.Mail == mail);
            return usuario;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista_usuarios = _context.Usuarios.Include(u => u.Venta).ToList();
            return lista_usuarios;
        }
    }
}
