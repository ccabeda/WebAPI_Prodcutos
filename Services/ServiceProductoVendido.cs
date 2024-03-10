using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using FluentValidation;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceProductoVendido : IServiceProductoVendido
    {
        private readonly IRepositoryProductoVendido _repository;
        private readonly IRepositoryVenta _repositoryVenta;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceProductoVendido> _logger;
        private readonly APIResponse _apiResponse;
        private readonly IValidator<ProductoVendidoCreateDto> _validator;
        private readonly IValidator<ProductoVendidoUpdateDto> _validatorUpdate;
        public ServiceProductoVendido(IRepositoryProductoVendido repository, IRepositoryProducto repositoryProducto, IRepositoryVenta repositoryVenta, IMapper mapper,
                                      ILogger<ServiceProductoVendido> logger, APIResponse apiResponse, IValidator<ProductoVendidoCreateDto> validator, IValidator<ProductoVendidoUpdateDto>
                                      validatorUpdate)
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
                var productSold = await _repository.GetById(id);//busco en la db con la id
                if (productSold == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<ProductoVendidoDTO>(productSold);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto Vendido: " + ex.Message);
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
                var listProductsSold = await _repository.GetAll();
                if (listProductsSold == null)
                {
                    _logger.LogError("No hay ningún producto vendido registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<ProductoVendidoDTO>>(listProductsSold);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos Vendidos: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Create(ProductoVendidoCreateDto productSoldCreate)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(productSoldCreate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existProductId = await _repositoryProducto.GetById(productSoldCreate.IdProducto);
                var existSaleId = await _repositoryVenta.GetById(productSoldCreate.IdVenta);
                if (existProductId == null || existSaleId == null)
                {
                    if (existProductId == null)
                    {
                        _logger.LogError("No existe producto con el idProducto enviado.");
                    }
                    else
                    {
                        _logger.LogError("No existe venta realizada con el idVenta enviado.");
                    }
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var productSold = _mapper.Map<ProductoVendido>(productSoldCreate);
                await _repository.Create(productSold!);
                _logger.LogInformation("!ProductoVendido creado con exito¡");
                _apiResponse.Result = _mapper.Map<ProductoVendidoDTO>(productSold);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Producto Vendido: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Update(ProductoVendidoUpdateDto productSoldUpdate)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(productSoldUpdate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existProductSold = await _repository.GetById(productSoldUpdate.Id);
                if (existProductSold == null)
                {
                    _logger.LogError("Error, el producto vendido que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existProductId = await _repositoryProducto.GetById(productSoldUpdate.IdProducto);
                var existSaleid = await _repositoryVenta.GetById(productSoldUpdate.IdVenta);
                if (existProductId == null || existSaleid == null)
                {
                    if (existProductId == null)
                    {
                        _logger.LogError("No existe producto con el idProdcuto enviado.");
                    }
                    else
                    {
                        _logger.LogError("No existe venta realizada con el idVenta enviado.");
                    }
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(productSoldUpdate, existProductSold);
                await _repository.Update(existProductSold);
                _logger.LogInformation("!El producto vendido de id " + productSoldUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Result = _mapper.Map<ProductoVendidoDTO>(existProductSold);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Producto Vendido: " + ex.Message);
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
                var productSold = await _repository.GetById(id);
                if (productSold == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                await _repository.Delete(productSold);
                _logger.LogInformation("¡Producto vendido eliminado con exito!");
                _apiResponse.Result = _mapper.Map<ProductoVendidoDTO>(productSold);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Producto Vendido: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> GetAllByUserId(int userId)
        {
            try
            {
                var listProductos = await _repositoryProducto.GetAllByUserId(userId); //obtengo todos los productos con el idusuario que enviaron
                if (listProductos.Count == 0) //si no hay ninguno entra aca
                {
                    _logger.LogError("No hay productos vendidos con ese idUsuario");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var finalList = new List<ProductoVendido>();
                var alreadyUsedIList = new List<int>(); //creo lista de ids ya usadas para no dar mas vueltas de las necesarias
                foreach (var i in listProductos) //de todos los productos del mismo idusuario
                {
                    if (!alreadyUsedIList.Contains(i.Id)) //si la id aun no se reviso
                    {
                        var resultado = await _repository.GetByProductId(i.Id); //busco y me quedo con los que allan sido vendidos
                        if (resultado != null)
                        {
                            finalList.AddRange(resultado); //agrego la lista a una nueva lista
                        }
                    }
                    alreadyUsedIList.Add(i.Id); //agrego el Id ya usado para que no vuelva a usarse
                }
                _apiResponse.Result = _mapper.Map<List<ProductoVendidoDTO>>(finalList); //los agrego a mi objeto de respuesta
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}