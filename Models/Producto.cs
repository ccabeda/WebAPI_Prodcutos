namespace Proyecto_Final.Models
{
    public class Producto
    {
        public Producto()
        {
            ProductoVendidos = new HashSet<ProductoVendido>();
        }
        public Producto(string descripcion, decimal costo, decimal precioventa,int stock, int idusuario)
        {
            Descripciones = descripcion;
            Costo = costo;
            PrecioVenta = precioventa;
            Stock = stock;
            IdUsuario = idusuario;
        }

        public int Id { get; set; }
        public string Descripciones { get; set; } = null!;
        public decimal? Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<ProductoVendido> ProductoVendidos { get; set; }
    }
}
