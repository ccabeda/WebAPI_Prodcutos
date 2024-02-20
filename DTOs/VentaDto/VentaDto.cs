using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using System.Text.Json.Serialization;

namespace WebApi_Proyecto_Final.DTOs.VentaDto
{
    public class VentaDto
    {

        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public  ICollection<ProductoVendidoDTO>? ProductoVendidos { get; set; }
    }
}
