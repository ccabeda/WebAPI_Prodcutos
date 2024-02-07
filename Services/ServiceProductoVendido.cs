using AutoMapper;
using Proyecto_Final.Models;
using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

namespace Proyecto_Final.Services
{
    public class ServiceProductoVendido : IServiceProductoVendido
    {
        private readonly IRepositoryGeneric<ProductoVendido> _repository;
        private readonly IRepositoryGeneric<Venta> _repositoryVenta;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceProductoVendido> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceProductoVendido(IRepositoryGeneric<ProductoVendido> repository, IRepositoryProducto repositoryProducto, IRepositoryGeneric<Venta> repositoryVenta, IMapper mapper, 
                                      ILogger<ServiceProductoVendido> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryVenta = repositoryVenta;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<APIResponse> ObtenerProductoVendido(int id)
        {
            try
            {
                var productoVendido = await _repository.ObtenerPorId(id);//busco en la db con la id
                if (productoVendido == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<ProductoVendidoDto>(productoVendido);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto Vendido: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ListarProductosVendidos()
        {
            try
            {
                var lista_productosVendidos = await _repository.ObtenerTodos();
                if (lista_productosVendidos == null)
                {
                    _logger.LogError("No hay ningún producto vendido registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<ProductoVendidoDto>>(lista_productosVendidos);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos Vendidos: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> CrearProductoVendido(ProductoVendidoCreateDto productoVendidoCreate)
        {
            try
            {
                if (productoVendidoCreate == null)
                {
                    _logger.LogError("Error al ingresar el producto vendido.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_idProducto = await _repositoryProducto.ObtenerPorId(productoVendidoCreate.IdProducto);
                if (existe_idProducto == null)
                {
                    _logger.LogError("No existe producto con el idProdcuto enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_idVenta = await _repositoryVenta.ObtenerPorId(productoVendidoCreate.IdVenta);
                if (existe_idVenta == null)
                {
                    _logger.LogError("No existe venta realizada con el idVenta enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var productoVendido = _mapper.Map<ProductoVendido>(productoVendidoCreate);
                await _repository.Crear(productoVendido);
                _logger.LogInformation("!ProductoVendido creado con exito¡");
                _apiResponse.Resultado = _mapper.Map<ProductoVendidoDto>(productoVendido);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Producto Vendido: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> ModificarProductoVendido(int id, ProductoVendidoUpdateDto productoVendidoUpdate)
        {
            try
            {
                var existeProductoVendido = await _repository.ObtenerPorId(id);
                if (existeProductoVendido == null)
                {
                    _logger.LogError("Error, el producto vendido que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_idProducto = await _repositoryProducto.ObtenerPorId(productoVendidoUpdate.IdProducto);
                if (existe_idProducto == null)
                {
                    _logger.LogError("No existe producto con el idProdcuto enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_idVenta = await _repositoryVenta.ObtenerPorId(productoVendidoUpdate.IdVenta);
                if (existe_idVenta == null)
                {
                    _logger.LogError("No existe venta realizada con el idVenta enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _mapper.Map(productoVendidoUpdate, existeProductoVendido);
                await _repository.Actualizar(existeProductoVendido);
                _logger.LogInformation("!El producto vendido de id " + id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoVendidoDto>(existeProductoVendido);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Producto Vendido: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> EliminarProductoVendido(int id)
        {
            try
            {
                var productoVendido = await _repository.ObtenerPorId(id);
                if (productoVendido == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                await _repository.Eliminar(productoVendido);
                _logger.LogInformation("¡Producto vendido eliminado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoVendidoDto>(productoVendido);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Producto Vendido: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}