using FluentValidation;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class ProductoVendidoUpdateValidator : AbstractValidator<ProductoVendidoUpdateDto>
    {
        public ProductoVendidoUpdateValidator()
        {
            RuleFor(p => p.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(p => p.Stock).NotEmpty().WithMessage("El stock no puede estar vacio.");
        }
    }
}
