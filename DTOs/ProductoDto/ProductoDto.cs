using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.DTOs.ProductoDto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Descripciones { get; set; } = null!;
        public decimal? Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int IdUsuario { get; set; }

        public virtual ICollection<ProductoVendidoDTO>? ProductoVendidos { get; set; }
    }
    public record ProdcutoCreateDto(int Id,
                                    string Descripciones,
                                    decimal? costo,
                                    decimal PrecioVenta,
                                    int Stock,
                                    int IdUsuario);
}
