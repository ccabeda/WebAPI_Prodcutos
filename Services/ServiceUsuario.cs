using AutoMapper;
using Proyecto_Final.Models;
using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

namespace Proyecto_Final.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryGeneric<Venta> _repositoryVenta;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceUsuario> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceUsuario(IRepositoryUsuario repository, IRepositoryProducto repositoryProducto, IRepositoryGeneric<Venta> repositoryVenta, IMapper mapper, ILogger<ServiceUsuario> logger, 
                              APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryVenta = repositoryVenta;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public APIResponse ObtenerUsuario(int id)
        {
            try
            {
                var usuario = _repository.ObtenerPorId(id); //busco en la db con la id
                if (usuario == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse ListarUsuarios()
        {
            try
            {
                var lista_usuarios = _repository.ObtenerTodos(); //traigo la lista de usuarios
                if (lista_usuarios == null)
                {
                    _logger.LogError("No hay ningún usuario registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<UsuarioDto>>(lista_usuarios);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Usuarios: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse CrearUsuario(UsuarioCreateDto usuarioCreate)
        {
            try
            {
                if (usuarioCreate == null)
                {
                    _logger.LogError("Error al ingresar el usuario.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_usuario = _repository.ObtenerPorNombre(usuarioCreate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                if (existe_usuario != null)
                {
                    _logger.LogError("El nombre de usuario " + usuarioCreate.NombreUsuario + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_mail = _repository.ObtenerPorMail(usuarioCreate.Mail);//si ya existe ese mail no deja crear
                if (existe_mail != null)
                {
                    _logger.LogError("El mail " + usuarioCreate.Mail + " ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreate);
                _repository.Crear(usuario);
                _logger.LogError("!Usuario creado con exito¡");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse ModificarUsuario(int id, UsuarioUpdateDto usuarioUpdate)
        {
            try
            {
                var existeUsuario = _repository.ObtenerPorId(id); //verifico que el id ingresado este registrado en la db
                if (existeUsuario == null)
                {
                    _logger.LogError("Error, el usuario que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var nombre_ya_registrado = _repository.ObtenerPorNombre(usuarioUpdate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != usuarioUpdate.Id)
                {
                    _logger.LogError("El nombre de usuario " + usuarioUpdate.NombreUsuario + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var mail_ya_registrado = _repository.ObtenerPorMail(usuarioUpdate.Mail); //si ya existe ese mail no deja crear
                if (mail_ya_registrado != null && mail_ya_registrado.Id != usuarioUpdate.Id)
                {
                    _logger.LogError("El mail " + usuarioUpdate.Mail + " ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _mapper.Map(usuarioUpdate, existeUsuario);
                _repository.Actualizar(existeUsuario);
                Console.WriteLine("!El usuario de id " + id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(existeUsuario);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse EliminarUsuario(int id)
        {
            try
            {
                var usuario = _repository.ObtenerPorId(id);
                if (usuario == null) //verifico que haya un usuario con ese id
                {
                    _logger.LogError("Error al intentar eliminar el usuario.");
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var lista_produtos = _repositoryProducto.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de producto
                                                                              //en el service de usuario
                foreach (var i in lista_produtos)
                {
                    if (i.Id == id)
                    {
                        _logger.LogError("Error. El Producto " + i.Descripciones + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina el producto para eliminar a este usuario.");
                        _apiResponse.FueExitoso = false;
                        return _apiResponse;
                    }
                }
                var lista_ventas = _repositoryVenta.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de venta
                                                                 //en el service de venta
                foreach (var i in lista_ventas)
                {
                    if (i.Id == id)
                    {
                        _logger.LogError("Error. La venta " + i.Comentarios + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina la venta para eliminar a este usuario.");
                        _apiResponse.FueExitoso = false;
                        return _apiResponse;
                    }
                }
                _repository.Eliminar(usuario);
                _logger.LogInformation("¡Usuario eliminado con exito!");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}