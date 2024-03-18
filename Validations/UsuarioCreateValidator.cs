using FluentValidation;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

namespace WebApi_Proyecto_Final.Validations
{
    public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateDto>
    {
        public UsuarioCreateValidator()
        {
            RuleFor(u => u.NombreUsuario).NotEmpty().WithMessage("El nombre de usuario no puede estar vacio.");
            RuleFor(u => u.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(u => u.Apellido).NotEmpty().WithMessage("El apellido no puede estar vacio.");
            RuleFor(u => u.Mail).EmailAddress().WithMessage("El Mail es incorrecto.");
            RuleFor(u => u.Contraseña).NotEmpty().WithMessage("La contraseña no puede estar vacio.").MinimumLength(6).WithMessage("La contraseña es muy corto.");
        }
    }
}
