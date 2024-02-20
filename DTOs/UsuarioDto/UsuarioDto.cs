using WebApi_Proyecto_Final.Models;

namespace WebApi_Proyecto_Final.DTOs.UsuarioDto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Mail { get; set; } = null!;

        public virtual ICollection<Venta>? Venta { get; set; }
    }
}
