using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.UnitOfWork;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; //para mapear a dtos
        private readonly ILogger<ServiceProducto> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceProducto(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceProducto> logger, APIResponse apiResponse)
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
                var product = await _unitOfWork.repositoryProducto.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(product))
                {
                    _logger.LogError("El producto de id " + id + " no se encuentra registrado.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var listProducts = await _unitOfWork.repositoryProducto.GetAll();
                if (Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts))
                {
                    _logger.LogError("La lista de Productos esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<ProductoDto, Producto>(_mapper, listProducts, _apiResponse);
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
                var existProduct = await _unitOfWork.repositoryProducto.GetByName(productCreate.Descripciones);
                var existUserId = await _unitOfWork.repositoryUsuario.GetById(productCreate.IdUsuario);
                if (!Utils.Utils.VerifyIfObjIsNull<Producto>(existProduct))
                {
                    _logger.LogError("El nombre " + productCreate.Descripciones + " ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId))
                {
                    _logger.LogError("El idUsuario " + productCreate.IdUsuario + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var product = _mapper.Map<Producto>(productCreate);
                await _unitOfWork.repositoryProducto.Create(product);
                await _unitOfWork.Save();
                _logger.LogInformation("!Producto creado con exito¡");
                return Utils.Utils.OKResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var product = await _unitOfWork.repositoryProducto.GetById(productUpdate.Id);
                var existUserId = await _unitOfWork.repositoryUsuario.GetById(productUpdate.IdUsuario);
                if (Utils.Utils.VerifyIfObjIsNull<Producto>(product))
                {
                    _logger.LogError("El producto de id " + productUpdate.Id + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId))
                {
                    _logger.LogError("El idUsuario " + productUpdate.IdUsuario + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var registredName = await _unitOfWork.repositoryProducto.GetByName(productUpdate.Descripciones);
                if (Utils.Utils.CheckIfNameAlreadyExist<Producto>(registredName, product))
                {
                    _logger.LogError( "El nombre" + productUpdate.Descripciones + " ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.Utils.ConflictResponse(_apiResponse);
                }
                _mapper.Map(productUpdate, product);
                await _unitOfWork.repositoryProducto.Update(product);
                await _unitOfWork.Save();
                _logger.LogInformation("!El producto de id " + productUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.OKResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var product = await _unitOfWork.repositoryProducto.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(product))
                {
                    _logger.LogError("El id enviado " + id + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var listProductSold = await _unitOfWork.repositoryproductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                                             //que seria llamando al repository de productos vendidos en el service de producto
                if (!Utils.Utils.PreventDeletionIfRelatedSoldProdcutExist<Producto>(product,listProductSold, id))
                {
                    _logger.LogError("El Producto no se puede eliminar porque hay un Producto Vendido que contiene como ProductoId este produto.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                await _unitOfWork.repositoryProducto.Delete(product);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Producto eliminado con exito!");
                return Utils.Utils.OKResponse<ProductoDto, Producto>(_mapper, product, _apiResponse);
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
                var listProducts = await _unitOfWork.repositoryProducto.GetAllByUserId(userId);
                if (Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts))
                {
                    _logger.LogError("La lista de Productos esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<ProductoDto, Producto>(_mapper, listProducts, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}