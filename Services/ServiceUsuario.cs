using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.UnitOfWork;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceUsuario> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceUsuario(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceUsuario> logger,
                              APIResponse apiResponse)
        {
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var user = await _unitOfWork.repositoryUsuario.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(user))
                {
                    _logger.LogError("El usuario de id " + id + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
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
                var listUser = await _unitOfWork.repositoryUsuario.GetAll();
                if (Utils.Utils.CheckIfLsitIsNull<Usuario>(listUser))
                {
                    _logger.LogError("La lista de usuarios esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<UsuarioDto, Usuario>(_mapper, listUser, _apiResponse);
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
                var existUser = await _unitOfWork.repositoryUsuario.GetByName(userCreate.NombreUsuario);
                var existMail = await _unitOfWork.repositoryUsuario.GetByMail(userCreate.Mail);
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existUser))
                {
                    _logger.LogError("El nombre de usuario " + userCreate.NombreUsuario + " ya existe. Utilize otro.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existMail))
                {
                    _logger.LogError("El mail " + userCreate.Mail + " ya se encuentra registrado.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                var user = _mapper.Map<Usuario>(userCreate);
                user.Contraseña = Encrypt.Encrypt.EncryptPassword(user.Contraseña); //encryptamos
                await _unitOfWork.repositoryUsuario.Create(user);
                await _unitOfWork.Save();
                _logger.LogError("!Usuario creado con exito¡");
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Update(UsuarioUpdateDto userUpdate, string username, string password)
        {
            try
            {
                var user = await _unitOfWork.repositoryUsuario.GetByName(username); //verifico que el id ingresado este registrado en la db
                if (Utils.Utils.VerifyIfObjIsNull<Usuario>(user))
                {
                    _logger.LogError("El usuario enviado no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyPassword(password, user.Contraseña))
                {
                    _logger.LogError("Contraseña incorrecta");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var registredName = await _unitOfWork.repositoryUsuario.GetByName(userUpdate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                var registredMail = await _unitOfWork.repositoryUsuario.GetByMail(userUpdate.Mail); //si ya existe ese mail no deja crear
                if (Utils.Utils.CheckIfNameAlreadyExist<Usuario>(registredName, user))
                {
                    _logger.LogError("El nombre de usuario " + userUpdate.NombreUsuario + " ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                if (Utils.Utils.CheckIfNameAlreadyExist<Usuario>(registredMail, user))
                {
                    _logger.LogError("El mail " + userUpdate.Mail + " ya se encuentra registrado.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                _mapper.Map(userUpdate, user);
                user.Contraseña = Encrypt.Encrypt.EncryptPassword(user.Contraseña); //encriptamos
                await _unitOfWork.repositoryUsuario.Update(user);
                await _unitOfWork.Save();
                Console.WriteLine("!El usuario " + username + " fue actualizado con exito!");
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Delete(string username, string password)
        {
            try
            {
                var user = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.Utils.VerifyIfObjIsNull(user))
                {
                    _logger.LogError("El usuario " + username + "no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyPassword(password, user.Contraseña))
                {
                    _logger.LogError("Contraseña incorrecta");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var listProducts = await _unitOfWork.repositoryProducto.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de producto
                                                                                              //en el service de usuario
                if (!Utils.Utils.PreventDeletionIfRelatedProductExist(listProducts, user.Id))
                {
                    _logger.LogError("El Usuario no se puede eliminar porque hay un Producto que contiene como UsuarioId este usuario.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var listSales = await _unitOfWork.repositoryVenta.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de venta
                if (!Utils.Utils.PreventDeletionIfRelatedSalesExist(listSales, user.Id))
                {
                    _logger.LogError("El Usuario no se puede eliminar porque hay una Venta que contiene como UsuarioId este usuario.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                await _unitOfWork.repositoryUsuario.Delete(user);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Usuario eliminado con exito!");
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
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
                var user = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.Utils.VerifyIfObjIsNull(user))
                {
                    _logger.LogError("El nombre de usuario " + username + "no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
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
                var user = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.Utils.VerifyIfObjIsNull(user))
                {
                    _logger.LogError("Usuario incorrecto.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyPassword(password, user.Contraseña))
                {
                    _logger.LogError("Contraseña incorrecta.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<UsuarioDto, Usuario>(_mapper, user, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}