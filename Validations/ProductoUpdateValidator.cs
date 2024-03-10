using FluentValidation;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class ProductoUpdateValidator : AbstractValidator<ProductoUpdateDto>
    {
        public ProductoUpdateValidator()
        {
            RuleFor(p => p.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(p => p.Descripciones).NotEmpty().WithMessage("La descripción no puede estar vacia.");
            RuleFor(p => p.Costo).NotEmpty().WithMessage("El costo no puede estar vacio.");
        }
    }
}
