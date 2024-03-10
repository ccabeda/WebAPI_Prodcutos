using FluentValidation;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class ProductoCreateValidator : AbstractValidator<ProductoCreateDto>
    {
        public ProductoCreateValidator() 
        {
            RuleFor(p => p.Descripciones).NotEmpty().WithMessage("La descripción no puede estar vacia.");
            RuleFor(p => p.Costo).NotEmpty().WithMessage("El costo no puede estar vacio.");
        }
    }
}
