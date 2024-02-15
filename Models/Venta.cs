using System.Text.Json.Serialization;

namespace Proyecto_Final.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }

        [JsonIgnore]
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<ProductoVendido>? ProductoVendidos { get; set; }
    }
}
