using Proyecto_Final.Models;
using Proyecto_Final.DTOs.ProductoVendidoDto;

namespace Proyecto_Final.DTOs.VentaDto
{
    public class VentaDto
    {

        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }

        public  ICollection<ProductoVendidoDTO>? ProductoVendidos { get; set; }
    }
}
