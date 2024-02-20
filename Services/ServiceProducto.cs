using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryProductoVendido _repositoryProductoVendido;
        private readonly IMapper _mapper; //para mapear a dtos
        private readonly ILogger<ServiceProducto> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceProducto(IRepositoryProducto repository, IRepositoryUsuario repositoryUsuario, IRepositoryProductoVendido repositoryProductoVendido, IMapper mapper,
                               ILogger<ServiceProducto> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<APIResponse> ObtenerProducto(int id)
        {
            try
            {
                var producto = await _repository.ObtenerPorId(id);
                if (producto == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarProductos()
        {
            try
            {
                var lista_productos = await _repository.ObtenerTodos();
                if (lista_productos == null)
                {
                    _logger.LogError("No hay ningún producto registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta= HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<ProductoDto>>(lista_productos);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> CrearProducto([FromBody] ProductoCreateDto productoCreate)
        {
            try
            {
                if (productoCreate == null)
                {
                    _logger.LogError("Error al ingresar el producto.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existeProducto = await _repository.ObtenerPorNombre(productoCreate.Descripciones);
                if (existeProducto != null)
                {
                    _logger.LogError("Un producto con ese nombre ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var existeidUsuario = await _repositoryUsuario.ObtenerPorId(productoCreate.IdUsuario);
                if (existeidUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                    return _apiResponse;
                }
                var producto = _mapper.Map<Producto>(productoCreate);
                await _repository.Crear(producto!);
                _logger.LogInformation("!Producto creado con exito¡");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ModificarProducto([FromBody] ProductoUpdateDto productoUpdate)
        {
            try
            {
                var existeProducto = await _repository.ObtenerPorId(productoUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existeProducto == null)
                {
                    _logger.LogError("Error, el producto que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var nombre_ya_registrado = await _repository.ObtenerPorNombre(productoUpdate.Descripciones);
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != productoUpdate.Id)
                {
                    _logger.LogError("El nombre del producto " + productoUpdate.Descripciones + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var existe_idUsuario = await _repositoryUsuario.ObtenerPorId(productoUpdate.IdUsuario);
                if (existe_idUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                    return _apiResponse;
                }
                _mapper.Map(productoUpdate, existeProducto);
                await _repository.Actualizar(existeProducto);
                _logger.LogInformation("!El producto de id " + productoUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(existeProducto);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> EliminarProducto(int id)
        {
            try
            {
                var producto = await _repository.ObtenerPorId(id);
                if (producto == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var lista_productos_vendidos = await _repositoryProductoVendido.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                                    //que seria llamando al repository de productos vendidos en el service de producto
                foreach (var i in lista_productos_vendidos)
                {
                    if (i.IdProducto == id)
                    {
                        _logger.LogError("Error. El producto vendido de id " + i.Id + " tiene como ProductoId a este producto.");
                        _logger.LogError("Modifica o elimina el producto vendido para eliminar a este producto.");
                        _apiResponse.FueExitoso = false;
                        _apiResponse.EstadoRespuesta = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Eliminar(producto);
                _logger.LogInformation("¡Producto eliminado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarProductosPorIdUsuario(int idUsuario)
        {
            try
            {
                var lista_productos = await _repository.ObtenerPorIdUsuario(idUsuario); 
                if (lista_productos.Count == 0)
                {
                    _logger.LogError("No hay ningún producto registrada a ese IdUsuario. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    _apiResponse.EstadoRespuesta = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<ProductoDto>>(lista_productos);
                _apiResponse.EstadoRespuesta = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos de ese IdUsuario: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.EstadoRespuesta = HttpStatusCode.NotFound;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}