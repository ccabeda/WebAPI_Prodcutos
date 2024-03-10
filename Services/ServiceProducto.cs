using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using FluentValidation;

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
        private readonly IValidator<ProductoCreateDto> _validator;
        private readonly IValidator<ProductoUpdateDto> _validatorUpdate;
        public ServiceProducto(IRepositoryProducto repository, IRepositoryUsuario repositoryUsuario, IRepositoryProductoVendido repositoryProductoVendido, IMapper mapper,
                               ILogger<ServiceProducto> logger, APIResponse apiResponse, IValidator<ProductoCreateDto> validator, IValidator<ProductoUpdateDto> validatorUpdate)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
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
                var product = await _repository.GetById(id);
                if (product == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<ProductoDto>(product);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener el Producto: " + ex.Message);
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
                var listProducts = await _repository.GetAll();
                if (listProducts == null)
                {
                    _logger.LogError("No hay ningún producto registrado actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode= HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<ProductoDto>>(listProducts);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Create([FromBody] ProductoCreateDto productCreate)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(productCreate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existProduct = await _repository.GetByName(productCreate.Descripciones);
                var existUserId = await _repositoryUsuario.GetById(productCreate.IdUsuario);
                if (existProduct != null || existUserId == null)
                {
                    if (existProduct != null)
                    {
                        _logger.LogError("Un producto con ese nombre ya se encuentra registrado.");
                    }
                    else
                    {
                        _logger.LogError("No existe usuario con el idUsuario enviado.");
                    }
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = existProduct != null ? HttpStatusCode.Conflict : HttpStatusCode.NotFound;
                    return _apiResponse;
                }
                var producto = _mapper.Map<Producto>(productCreate);
                await _repository.Create(producto!);
                _logger.LogInformation("!Producto creado con exito¡");
                _apiResponse.Result = _mapper.Map<ProductoDto>(producto);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Producto: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Update([FromBody] ProductoUpdateDto productUpdate)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(productUpdate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existProduct = await _repository.GetById(productUpdate.Id); // Verifico que el id ingresado esté registrado en la base de datos
                var existUserId = await _repositoryUsuario.GetById(productUpdate.IdUsuario);
                if (existProduct == null || existUserId == null)
                {
                    if (existProduct == null)
                    {
                        _logger.LogError("Error, el producto que intenta modificar no existe.");
                        _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    }
                    else
                    {
                        _logger.LogError("No existe usuario con el idUsuario enviado.");
                    }
                    _apiResponse.StatusCode = existProduct == null ? HttpStatusCode.BadRequest : HttpStatusCode.NotFound;
                    _apiResponse.IsExit = false;
                    return _apiResponse;
                }
                var registredName = await _repository.GetByName(productUpdate.Descripciones);
                if (registredName != null && registredName.Id != productUpdate.Id)
                {
                    _logger.LogError("El nombre del producto " + productUpdate.Descripciones + " ya existe. Utilize otro.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(productUpdate, existProduct);
                await _repository.Update(existProduct);
                _logger.LogInformation("!El producto de id " + productUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Result = _mapper.Map<ProductoDto>(existProduct);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el Producto: " + ex.Message);
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
                var product = await _repository.GetById(id);
                if (product == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var listProductSold = await _repositoryProductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                                    //que seria llamando al repository de productos vendidos en el service de producto
                foreach (var i in listProductSold)
                {
                    if (i.IdProducto == id)
                    {
                        _logger.LogError("Error. El producto vendido de id " + i.Id + " tiene como ProductoId a este producto.");
                        _logger.LogError("Modifica o elimina el producto vendido para eliminar a este producto.");
                        _apiResponse.IsExit = false;
                        _apiResponse.StatusCode = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Delete(product);
                _logger.LogInformation("¡Producto eliminado con exito!");
                _apiResponse.Result = _mapper.Map<ProductoDto>(product);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Producto: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> GetAllByUserId(int userId)
        {
            try
            {
                var listProducts = await _repository.GetAllByUserId(userId); 
                if (listProducts.Count == 0)
                {
                    _logger.LogError("No hay ningún producto registrada a ese IdUsuario. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<ProductoDto>>(listProducts);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Productos de ese IdUsuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}