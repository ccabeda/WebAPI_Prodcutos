using Proyecto_Final.Models;
using System.Text.Json.Serialization;

namespace WebApi_Proyecto_Final.DTOs.VentaDto
{
    public class VentaDto
    {

        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }

        public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; }
    }
}
