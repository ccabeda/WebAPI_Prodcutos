using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
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

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var product = await _repository.GetById(id);
                if (!Utils.Utils.VerifyIfObjIsNull(product, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var listProducts = await _repository.GetAll();
                if (!Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<ProductoDto, Producto>(_mapper, listProducts, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] ProductoCreateDto productCreate)
        {
            try
            {
                var existProduct = await _repository.GetByName(productCreate.Descripciones);
                var existUserId = await _repositoryUsuario.GetById(productCreate.IdUsuario);
                if (!Utils.Utils.CheckIfObjectExist<Producto>(existProduct, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId, _apiResponse, _logger))
                {
                    _logger.LogError("El idUsuario no se encuentra registrado");
                    return _apiResponse;
                }
                var product = _mapper.Map<Producto>(productCreate);
                await _repository.Create(product!);
                _logger.LogInformation("!Producto creado con exito¡");
                return Utils.Utils.CorrectResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] ProductoUpdateDto productUpdate)
        {
            try
            {
                var product = await _repository.GetById(productUpdate.Id); // Verifico que el id ingresado esté registrado en la base de datos
                var existUserId = await _repositoryUsuario.GetById(productUpdate.IdUsuario);
                if (!Utils.Utils.VerifyIfObjIsNull<Producto>(product, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId, _apiResponse, _logger))
                {
                    _logger.LogError("El idUsuario no se encuentra registrado");
                    return _apiResponse;
                }
                var registredName = await _repository.GetByName(productUpdate.Descripciones);
                if (!Utils.Utils.CheckIfNameAlreadyExist<Producto>(registredName, product, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                _mapper.Map(productUpdate, product);
                await _repository.Update(product);
                _logger.LogInformation("!El producto de id " + productUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.CorrectResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var product = await _repository.GetById(id);
                if (!Utils.Utils.VerifyIfObjIsNull(product, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var listProductSold = await _repositoryProductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                                    //que seria llamando al repository de productos vendidos en el service de producto

                if(!Utils.Utils.PreventDeletionIfRelatedSoldProdcutExist<Producto>(product,listProductSold, _apiResponse, id))
                {
                    _logger.LogError("El Producto no se puede eliminar porque hay un Producto Vendido que contiene como ProductoId este produto.");
                    return _apiResponse;
                }
                await _repository.Delete(product);
                _logger.LogInformation("¡Producto eliminado con exito!");
                return Utils.Utils.CorrectResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> GetAllByUserId(int userId)
        {
            try
            {
                var listProducts = await _repository.GetAllByUserId(userId);
                if (!Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<ProductoDto, Producto>(_mapper, listProducts, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}