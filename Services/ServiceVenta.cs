using AutoMapper;
using Proyecto_Final.Models;
using Proyecto_Final.Models.APIResponse;
using System.Net;
using Proyecto_Final.DTOs.VentaDto;
using Proyecto_Final.Repository.IRepository;
using Proyecto_Final.Services.IService;

namespace Proyecto_Final.Services
{
    public class ServiceVenta : IServiceVenta
    {
        private readonly IRepositoryVenta _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryGeneric<ProductoVendido> _repositoryProductoVendido;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceVenta> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceVenta(IRepositoryVenta repository, IRepositoryUsuario repositoryUsuario, IRepositoryGeneric<ProductoVendido> repositoryProductoVendido, IMapper mapper,
                            ILogger<ServiceVenta> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<APIResponse> ObtenerVenta(int id)
        {
            try
            {
                var venta = await _repository.ObtenerPorId(id); //busco en la db con la id
                if (venta == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<VentaDto>(venta);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la Venta: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarVentas()
        {
            try
            {
                var lista_Ventas = await _repository.ObtenerTodos(); //traigo la lista de usuarios
                if (lista_Ventas == null)
                {
                    _logger.LogError("No hay ningúna venta registrada actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<VentaDto>>(lista_Ventas);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Ventas: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> CrearVenta(VentaCreateDto ventaCreate)
        {
            try
            {
                if (ventaCreate == null)
                {
                    _logger.LogError("Error al ingresar la venta.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existe_idUsuario = await _repositoryUsuario.ObtenerPorId(ventaCreate.IdUsuario);
                if (existe_idUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var venta = _mapper.Map<Venta>(ventaCreate);
                await _repository.Crear(venta!);
                _logger.LogError("!Venta creada con exito¡");
                _apiResponse.Resultado = _mapper.Map<VentaDto>(venta);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear la Venta: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ModificarVenta(VentaUpdateDto ventaUpdate)
        {
            try
            {
                var existeVenta = await _repository.ObtenerPorId(ventaUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existeVenta == null)
                {
                    _logger.LogError("Error, la venta que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existe_idUsuario = await _repositoryUsuario.ObtenerPorId(ventaUpdate.IdUsuario);
                if (existe_idUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(ventaUpdate, existeVenta);
                await _repository.Actualizar(existeVenta); //guardo cambios
                _logger.LogInformation("!La venta de id " + ventaUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<VentaDto>(existeVenta);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar la Venta: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> EliminarVenta(int id)
        {
            try
            {
                var venta = await _repository.ObtenerPorId(id);
                if (venta == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var lista_productos_vendidos = await _repositoryProductoVendido.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                   //que seria llamando al repository de productos vendidos en el service de venta
                foreach (var i in lista_productos_vendidos)
                {
                    if (i.IdVenta == id)
                    {
                        _logger.LogError("Error. El producto vendido de id " + i.Id + " tiene como VentaId a esta venta.");
                        _logger.LogError("Modifica o elimina el producto vendido para eliminar esta venta.");
                        _apiResponse.FueExitoso = false;
                        _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Eliminar(venta);
                _logger.LogInformation("¡Venta eliminada con exito!");
                _apiResponse.Resultado = _mapper.Map<VentaDto>(venta);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar la Venta: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarVentasPorIdUsuario(int idUsuario)
        {
            try
            {
                var lista_Ventas = await _repository.ObtenerPorIdUsuario(idUsuario); //traigo la lista de usuarios
                if (lista_Ventas.Count == 0)
                {
                    _logger.LogError("No hay ningúna venta registrada a ese IdUsuario. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<VentaDto>>(lista_Ventas);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Ventas de ese IdUsuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}