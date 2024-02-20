using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

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
        public ServiceUsuario(IRepositoryUsuario repository, IRepositoryProducto repositoryProducto, IRepositoryVenta repositoryVenta, IMapper mapper, ILogger<ServiceUsuario> logger,
                              APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryVenta = repositoryVenta;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<APIResponse> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _repository.ObtenerPorId(id); //busco en la db con la id
                if (usuario == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarUsuarios()
        {
            try
            {
                var lista_usuarios = await _repository.ObtenerTodos(); //traigo la lista de usuarios
                if (lista_usuarios == null)
                {
                    _logger.LogError("No hay ningún usuario registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<UsuarioDto>>(lista_usuarios);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Usuarios: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> CrearUsuario(UsuarioCreateDto usuarioCreate)
        {
            try
            {
                if (usuarioCreate == null)
                {
                    _logger.LogError("Error al ingresar el usuario.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existe_usuario = await _repository.ObtenerPorNombre(usuarioCreate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                if (existe_usuario != null)
                {
                    _logger.LogError("El nombre de usuario " + usuarioCreate.NombreUsuario + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var existe_mail = await _repository.ObtenerPorMail(usuarioCreate.Mail);//si ya existe ese mail no deja crear
                if (existe_mail != null)
                {
                    _logger.LogError("El mail " + usuarioCreate.Mail + " ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreate);
                await _repository.Crear(usuario!);
                _logger.LogError("!Usuario creado con exito¡");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ModificarUsuario(UsuarioUpdateDto usuarioUpdate)
        {
            try
            {
                var existeUsuario = await _repository.ObtenerPorId(usuarioUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existeUsuario == null)
                {
                    _logger.LogError("Error, el usuario que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var nombre_ya_registrado = await _repository.ObtenerPorNombre(usuarioUpdate.NombreUsuario); //si ya existe ese nombredeusuario no deja crear
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != usuarioUpdate.Id) //agrego que el id sea diferente a el id de el mismo para que no se trackee a el mismo
                {
                    _logger.LogError("El nombre de usuario " + usuarioUpdate.NombreUsuario + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var mail_ya_registrado = await _repository.ObtenerPorMail(usuarioUpdate.Mail); //si ya existe ese mail no deja crear
                if (mail_ya_registrado != null && mail_ya_registrado.Id != usuarioUpdate.Id)
                {
                    _logger.LogError("El mail " + usuarioUpdate.Mail + " ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(usuarioUpdate, existeUsuario);
                await _repository.Actualizar(existeUsuario);
                Console.WriteLine("!El usuario de id " + usuarioUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(existeUsuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _repository.ObtenerPorId(id);
                if (usuario == null) //verifico que haya un usuario con ese id
                {
                    _logger.LogError("Error al intentar eliminar el usuario.");
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var lista_produtos = await _repositoryProducto.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de producto
                                                                               //en el service de usuario
                foreach (var i in lista_produtos)
                {
                    if (i.Id == id)
                    {
                        _logger.LogError("Error. El Producto " + i.Descripciones + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina el producto para eliminar a este usuario.");
                        _apiResponse.FueExitoso = false;
                        _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                var lista_ventas = await _repositoryVenta.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa, que seria llamando al repository de venta
                                                                          //en el service de venta
                foreach (var i in lista_ventas)
                {
                    if (i.Id == id)
                    {
                        _logger.LogError("Error. La venta " + i.Comentarios + " de id " + i.Id + " tiene como UsuarioId a este usuario.");
                        _logger.LogError("Modifica o elimina la venta para eliminar a este usuario.");
                        _apiResponse.FueExitoso = false;
                        _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Eliminar(usuario);
                _logger.LogInformation("¡Usuario eliminado con exito!");
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ObtenerUsuarioPorNombreUsuario(string username)
        {
            try
            {
                var usuario = await _repository.ObtenerPorNombre(username); //busco en la db con la id
                if (usuario == null)
                {
                    _logger.LogError("Error, el usuario ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> IniciarSesion(string username, string password)
        {
            try
            {
                var usuario = await _repository.ObtenerPorNombre(username); //busco en la db con la id
                if (usuario == null)
                {
                    _logger.LogError("Error, el usuario ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                if (usuario.Contraseña != password)
                {
                    _logger.LogError("Error,contraseña incorrecta.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<UsuarioDto>(usuario);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Usuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}