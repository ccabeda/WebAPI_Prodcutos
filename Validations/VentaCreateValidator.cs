using FluentValidation;
using WebApi_Proyecto_Final.DTOs.VentaDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class VentaCreateValidator : AbstractValidator<VentaCreateDto>
    {
        public VentaCreateValidator()
        {
            RuleFor(v => v.Comentarios).NotEmpty().WithMessage("El comentario no puede estar vacio.");
        }
    }
}
