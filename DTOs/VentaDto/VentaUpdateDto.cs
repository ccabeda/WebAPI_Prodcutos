namespace WebApi_Proyecto_Final.DTOs.VentaDto
{
    public class VentaUpdateDto
    {
        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }
    }
}
