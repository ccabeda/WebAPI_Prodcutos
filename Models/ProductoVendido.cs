using System.Text.Json.Serialization;

namespace Proyecto_Final.Models
{
    public class ProductoVendido
    {
        public ProductoVendido()
        {
            
        }
        public ProductoVendido(int stock, int idProducto, int idVenta)
        {
            Stock = stock;
            IdProducto = idProducto;
            IdVenta = idVenta;
        }

        public int Id { get; set; }
        public int Stock { get; set; }
        public int IdProducto { get; set; }
        public int IdVenta { get; set; }

        [JsonIgnore]
        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Venta IdVentaNavigation { get; set; } = null!;
    }
}
