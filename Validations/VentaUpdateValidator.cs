using FluentValidation;
using WebApi_Proyecto_Final.DTOs.VentaDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class VentaUpdateValidator : AbstractValidator<VentaUpdateDto>
    {
        public VentaUpdateValidator()
        {
            RuleFor(v => v.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(v => v.Comentarios).NotEmpty().WithMessage("El comentario no puede estar vacio.");
        }
    }
}
