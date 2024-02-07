namespace Proyecto_Final.Models
{
    public class Usuario
    {
        public Usuario()
        {
            Productos = new HashSet<Producto>();
            Venta = new HashSet<Venta>();
        }
        public Usuario(string nombre, string apellido, string nombreusuario, string contraseña, string mail)
        {
            Nombre = nombre;
            Apellido = apellido;
            NombreUsuario = nombreusuario;
            Contraseña = contraseña;
            Mail = mail;
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Mail { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
