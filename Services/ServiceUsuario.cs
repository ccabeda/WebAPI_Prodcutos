using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using FluentValidation;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryVenta _repositoryVenta;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceUsuario> _logger;
        private readonly APIResponse _apiResponse;
        private readonly IValidator<UsuarioCreateDto> _validator;
        private readonly IValidator<UsuarioUpdateDto> _validatorUpdate;
        public ServiceUsuario(IRepositoryUsuario repository, IRepositoryProducto repositoryProducto, IRepositoryVenta repositoryVenta, IMapper mapper, ILogger<ServiceUsuario> logger,
                              APIResponse apiResponse, IValidator<UsuarioCreateDto> validator,IValidator<UsuarioUpdateDto> validatorUpdate)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryVenta = repositoryVenta;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var user = await _repository.GetById(id); //busco en la db con la id
                if (user == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<UsuarioDto>(user);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                var listUser = await _repository.GetAll(); //traigo la lista de usuarios
                if (listUser == null)
                {
                    _logger.LogError("No hay ningún usuario registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<UsuarioDto>>(listUser);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Usuarios: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Create(UsuarioCreateDto userCreate)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(userCreate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existUser = await _repository.GetByName(userCreate.NombreUsuario);
                var existMail = await _repository.GetByMail(userCreate.Mail);
                if (existUser != null || existMail != null)
                {
                    if (existUser != null)
                    {
                        _logger.LogError("El nombre de usuario " + userCreate.NombreUsuario + " ya existe. Utilize otro.");
                    }
                    else
                    {
                        _logger.LogError("El mail " + userCreate.Mail + " ya se encuentra registrado.");
                    }
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var user = _mapper.Map<Usuario>(userCreate);
                await _repository.Create(user);
                _logger.LogError("!Usuario creado con exito¡");
                _apiResponse.Result = _mapper.Map<UsuarioDto>(user);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Update(UsuarioUpdateDto userUpdate)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(userUpdate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existeUsuario = await _repository.GetById(userUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existeUsuario == null)
                {
                    _logger.LogError("Error, el usuario que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var registredName = await _repository.GetByName(userUpdate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                if (registredName != null && registredName.Id != userUpdate.Id) //agrego que el id sea diferente a el id de el mismo para que no se trackee a el mismo
                {
                    _logger.LogError("El nombre de usuario " + userUpdate.NombreUsuario + " ya existe. Utilize otro.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var registredMail = await _repository.GetByMail(userUpdate.Mail); //si ya existe ese mail no deja crear
                if (registredMail != null && registredMail.Id != userUpdate.Id)
                {
                    _logger.LogError("El mail " + userUpdate.Mail + " ya se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(userUpdate, existeUsuario);
                await _repository.Update(existeUsuario);
                Console.WriteLine("!El usuario de id " + userUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Result = _mapper.Map<UsuarioDto>(existeUsuario);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Delete(int id)
        {
            try
            {
                var user = await _repository.GetById(id);
                if (user == null) //verifico que haya un usuario con ese id
                {
                    _logger.LogError("Error al intentar eliminar el usuario.");
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var listProducts = await _repositoryProducto.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de producto
                                                                               //en el service de usuario
                foreach (var i in listProducts)
                {
                    if (i.IdUsuario == id)
                    {
                        _logger.LogError("Error. El Producto " + i.Descripciones + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina el producto para eliminar a este usuario.");
                        _apiResponse.IsExit = false;
                        _apiResponse.StatusCode = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                var listSales = await _repositoryVenta.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de venta
                                                                          //en el service de venta
                foreach (var i in listSales)
                {
                    if (i.IdUsuario == id)
                    {
                        _logger.LogError("Error. La venta " + i.Comentarios + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina la venta para eliminar a este usuario.");
                        _apiResponse.IsExit = false;
                        _apiResponse.StatusCode = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Delete(user);
                _logger.LogInformation("¡Usuario eliminado con exito!");
                _apiResponse.Result = _mapper.Map<UsuarioDto>(user);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> GetByUsername(string username)
        {
            try
            {
                var user = await _repository.GetByName(username); //busco en la db con la id
                if (user == null)
                {
                    _logger.LogError("Error, el usuario ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<UsuarioDto>(user);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Result = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Login(string username, string password)
        {
            try
            {
                var user = await _repository.GetByName(username); //busco en la db con la id
                if (user == null)
                {
                    _logger.LogError("Error, el usuario ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                if (user.Contraseña != password)
                {
                    _logger.LogError("Error,contraseña incorrecta.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<UsuarioDto>(user);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}