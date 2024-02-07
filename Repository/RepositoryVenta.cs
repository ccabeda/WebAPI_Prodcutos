using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Database;
using Proyecto_Final.Models;
using WebApi_Proyecto_Final.Repository.IRepository;

namespace WebApi_Proyecto_Final.Repository
{
    public class RepositoryVenta : IRepositoryGeneric<Venta>
    {
        private readonly AplicationDbContext _context;
        public RepositoryVenta(AplicationDbContext context)
        {
            _context = context;
        }

        public void Actualizar(Venta venta)
        {
            _context.Update(venta);
            Guardar();
        }

        public void Crear(Venta venta)
        {
            _context.Venta.Add(venta);
            Guardar();
        }

        public void Eliminar(Venta venta)
        {
            _context.Venta.Remove(venta);
            _context.SaveChanges();
        }

        public void Guardar()
        {
            _context.SaveChanges();
        }

        public Venta ObtenerPorId(int id)
        {
            var venta = _context.Venta.Include(v => v.ProductoVendidos).FirstOrDefault(v => v.Id == id);
            return venta;
        }

        public List<Venta> ObtenerTodos()
        {
            var lista_ventas = _context.Venta.Include(v => v.ProductoVendidos).ToList();
            return lista_ventas;
        }
    }
}
