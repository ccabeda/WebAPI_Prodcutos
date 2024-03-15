using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using FluentValidation;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;

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
                if (!Utils.Utils.VerifyIfObjIsNull(user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                var listUser = await _repository.GetAll(); //traigo la lista de usuarios
                if (!Utils.Utils.CheckIfLsitIsNull<Usuario>(listUser, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<UsuarioDto, Usuario>(_mapper, listUser, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Create(UsuarioCreateDto userCreate)
        {
            try
            {
                if (await Utils.Utils.FluentValidator(userCreate, _validator, _apiResponse, _logger) != null)
                {
                    return _apiResponse;
                }
                var existUser = await _repository.GetByName(userCreate.NombreUsuario);
                var existMail = await _repository.GetByMail(userCreate.Mail);
                if (!Utils.Utils.CheckIfObjectExist<Usuario>(existUser, _apiResponse, _logger))
                {
                    _logger.LogError("El nombre de usuario " + userCreate.NombreUsuario + " ya existe. Utilize otro.");
                    return _apiResponse;
                }
                if (!Utils.Utils.CheckIfObjectExist<Usuario>(existMail, _apiResponse, _logger))
                {
                    _logger.LogError("El mail " + userCreate.Mail + " ya se encuentra registrado.");
                    return _apiResponse;
                }
                var user = _mapper.Map<Usuario>(userCreate);
                await _repository.Create(user);
                _logger.LogError("!Usuario creado con exito¡");
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Update(UsuarioUpdateDto userUpdate)
        {
            try
            {
                if (await Utils.Utils.FluentValidator(userUpdate, _validatorUpdate, _apiResponse, _logger) != null)
                {
                    return _apiResponse;
                }
                var user = await _repository.GetById(userUpdate.Id); //verifico que el id ingresado este registrado en la db
                if(!Utils.Utils.VerifyIfObjIsNull<Usuario>(user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var registredName = await _repository.GetByName(userUpdate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                var registredMail = await _repository.GetByMail(userUpdate.Mail); //si ya existe ese mail no deja crear
                if (!Utils.Utils.CheckIfNameAlreadyExist<Usuario>(registredName, user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.CheckIfNameAlreadyExist<Usuario>(registredMail, user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                _mapper.Map(userUpdate, user);
                await _repository.Update(user);
                Console.WriteLine("!El usuario de id " + userUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Delete(int id)
        {
            try
            {
                var user = await _repository.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var listProducts = await _repositoryProducto.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de producto
                                                                               //en el service de usuario
                if (!Utils.Utils.PreventDeletionIfRelatedProductExist(listProducts, _apiResponse, id))
                {
                    _logger.LogError("El Usuario no se puede eliminar porque hay un Producto que contiene como UsuarioId este usuario.");
                    return _apiResponse;
                }
                var listSales = await _repositoryVenta.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de venta
                if(!Utils.Utils.PreventDeletionIfRelatedSalesExist(listSales, _apiResponse, id))
                {
                    _logger.LogError("El Usuario no se puede eliminar porque hay una Venta que contiene como UsuarioId este usuario.");
                    return _apiResponse;
                }
                await _repository.Delete(user);
                _logger.LogInformation("¡Usuario eliminado con exito!");
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> GetByUsername(string username)
        {
            try
            {
                var user = await _repository.GetByName(username); //busco en la db con la id
                if (Utils.Utils.VerifyIfObjIsNull(user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Login(string username, string password)
        {
            try
            {
                var user = await _repository.GetByName(username); //busco en la db con la id
                if (Utils.Utils.VerifyIfObjIsNull(user, _apiResponse, _logger))
                {
                    return _apiResponse;
                }

                if (!Utils.Utils.VerifyPassword(user.Contraseña, password, _logger, _apiResponse))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}