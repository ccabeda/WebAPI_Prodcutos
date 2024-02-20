namespace WebApi_Proyecto_Final.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Mail { get; set; } = null!;

        public virtual ICollection<Producto>? Productos { get; set; }
        public virtual ICollection<Venta>? Venta { get; set; }
    }
}
