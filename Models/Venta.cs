using System.Text.Json.Serialization;

namespace Proyecto_Final.Models
{
    public class Venta
    {
        public Venta()
        {
            ProductoVendidos = new HashSet<ProductoVendido>();
        }
        public Venta(string comentarios, int idUsuario)
        {
            Comentarios = comentarios;
            IdUsuario = idUsuario;
        } 

        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }

        [JsonIgnore]
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; }
    }
}
