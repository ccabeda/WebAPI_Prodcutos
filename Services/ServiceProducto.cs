using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
using Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

namespace Proyecto_Final.Services
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryGeneric<ProductoVendido> _repositoryProductoVendido;
        private readonly IMapper _mapper; //para mapear a dtos
        private readonly ILogger<ServiceProducto> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceProducto(IRepositoryProducto repository, IRepositoryUsuario repositoryUsuario, IRepositoryGeneric<ProductoVendido> repositoryProductoVendido, IMapper mapper,
                               ILogger<ServiceProducto> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public APIResponse ObtenerProducto(int id)
        {
            try
            {
                var producto = _repository.ObtenerPorId(id);
                if (producto == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse ListarProductos()
        {
            try
            {
                var lista_productos = _repository.ObtenerTodos();
                if (lista_productos == null)
                {
                    _logger.LogError("No hay ningún producto registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _apiResponse.Resultado = _mapper.Map<IEnumerable<ProductoDto>>(lista_productos);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse CrearProducto([FromBody] ProductoCreateDto productoCreate)
        {
            try
            {
                if (productoCreate == null)
                {
                    _logger.LogError("Error al ingresar el producto.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existeProducto = _repository.ObtenerPorNombre(productoCreate.Descripciones);
                if (existeProducto != null)
                {
                    _logger.LogError("Un producto con ese nombre ya se encuentra registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existeidUsuario = _repositoryUsuario.ObtenerPorId(productoCreate.IdUsuario);
                if (existeidUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var producto = _mapper.Map<Producto>(productoCreate);
                _repository.Crear(producto);
                _logger.LogInformation("!Producto creado con exito¡");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse ModificarProducto(int id, [FromBody] ProductoUpdateDto productoUpdate)
        {
            try
            {
                var existeProducto = _repository.ObtenerPorId(productoUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existeProducto == null)
                {
                    _logger.LogError("Error, el producto que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var nombre_ya_registrado = _repository.ObtenerPorNombre(productoUpdate.Descripciones);
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != productoUpdate.Id)
                {
                    _logger.LogError("El nombre del producto " + productoUpdate.Descripciones + " ya existe. Utilize otro.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var existe_idUsuario = _repositoryUsuario.ObtenerPorId(productoUpdate.IdUsuario);
                if (existe_idUsuario == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                _mapper.Map(productoUpdate, existeProducto);
                _repository.Actualizar(existeProducto);
                _logger.LogInformation("!El producto de id " + id + " fue actualizado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(existeProducto);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public APIResponse EliminarProducto(int id)
        {
            try
            {
                var producto = _repository.ObtenerPorId(id);
                if (producto == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.FueExitoso = false;
                    return _apiResponse;
                }
                var lista_productos_vendidos = _repositoryProductoVendido.ObtenerTodos(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                                    //que seria llamando al repository de productos vendidos en el service de producto
                foreach (var i in lista_productos_vendidos)
                {
                    if (i.Id == id)
                    {
                        _logger.LogError("Error. El producto vendido de id " + i.Id + " tiene como ProductoId a este producto.");
                        _logger.LogError("Modifica o elimina el producto vendido para eliminar a este producto.");
                        _apiResponse.FueExitoso = false;
                        return _apiResponse;
                    }
                }
                _repository.Eliminar(producto);
                _logger.LogInformation("¡Producto eliminado con exito!");
                _apiResponse.Resultado = _mapper.Map<ProductoDto>(producto);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Producto: " + ex.Message);
                _apiResponse.FueExitoso = false;
                _apiResponse.Exepciones = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}