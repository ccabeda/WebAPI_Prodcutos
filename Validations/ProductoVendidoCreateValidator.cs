using FluentValidation;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class ProductoVendidoCreateValidator : AbstractValidator<ProductoVendidoCreateDto>
    {
        public ProductoVendidoCreateValidator() 
        {
            RuleFor(p => p.Stock).NotEmpty().WithMessage("El stock no puede estar vacio.");
        }
    }
}
