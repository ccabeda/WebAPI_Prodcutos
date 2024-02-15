using AutoMapper;
using Proyecto_Final.Models;
using Proyecto_Final.DTOs.ProductoDto;
using Proyecto_Final.DTOs.ProductoVendidoDto;
using Proyecto_Final.DTOs.UsuarioDto;
using Proyecto_Final.DTOs.VentaDto;

namespace Proyecto_Final.Mappers
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Producto, ProductoCreateDto>().ReverseMap();
            CreateMap<Producto, ProductoUpdateDto>().ReverseMap();
            CreateMap<ProductoVendido, ProductoVendidoDTO>().ReverseMap();
            CreateMap<ProductoVendido, ProductoVendidoCreateDto>().ReverseMap();
            CreateMap<ProductoVendido, ProductoVendidoUpdateDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioCreateDto>().ReverseMap();
            CreateMap<Usuario, UsuarioUpdateDto>().ReverseMap();
            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<Venta, VentaCreateDto>().ReverseMap();
            CreateMap<Venta, VentaUpdateDto>().ReverseMap();
        }
    }
}
