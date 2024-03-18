using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;

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
        public ServiceProductoVendido(IRepositoryProductoVendido repository, IRepositoryProducto repositoryProducto, IRepositoryVenta repositoryVenta, IMapper mapper,
                                      ILogger<ServiceProductoVendido> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryVenta = repositoryVenta;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var productSold = await _repository.GetById(id);//busco en la db con la id
                if (!Utils.Utils.VerifyIfObjIsNull(productSold, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var listProductsSold = await _repository.GetAll();
                if (!Utils.Utils.CheckIfLsitIsNull<ProductoVendido>(listProductsSold, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, listProductsSold, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Create(ProductoVendidoCreateDto productSoldCreate)
        {
            try
            {
                var existProductId = await _repositoryProducto.GetById(productSoldCreate.IdProducto);
                var existSaleId = await _repositoryVenta.GetById(productSoldCreate.IdVenta);
                if (!Utils.Utils.VerifyIfObjIsNull<Producto>(existProductId, _apiResponse, _logger))
                {
                    _logger.LogError("El idProducto no se encuentra registrado");
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Venta>(existSaleId, _apiResponse, _logger))
                {
                    _logger.LogError("El idVenta no se encuentra registrado");
                    return _apiResponse;
                }
                var productSold = _mapper.Map<ProductoVendido>(productSoldCreate);
                await _repository.Create(productSold!);
                _logger.LogInformation("!ProductoVendido creado con exito¡");
                return Utils.Utils.CorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Update(ProductoVendidoUpdateDto productSoldUpdate)
        {
            try
            {
                var productSold = await _repository.GetById(productSoldUpdate.Id);
                var existProductId = await _repositoryProducto.GetById(productSoldUpdate.IdProducto);
                var existSaleId = await _repositoryVenta.GetById(productSoldUpdate.IdVenta);
                if (!Utils.Utils.VerifyIfObjIsNull<ProductoVendido>(productSold, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Producto>(existProductId, _apiResponse, _logger))
                {
                    _logger.LogError("El idProducto no se encuentra registrado");
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Venta>(existSaleId, _apiResponse, _logger))
                {
                    _logger.LogError("El idVenta no se encuentra registrado");
                    return _apiResponse;
                }
                _mapper.Map(productSoldUpdate, productSold);
                await _repository.Update(productSold);
                _logger.LogInformation("!El producto vendido de id " + productSoldUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.CorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var productSold = await _repository.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(productSold, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                await _repository.Delete(productSold);
                _logger.LogInformation("¡Producto vendido eliminado con exito!");
                return Utils.Utils.CorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var listProducts = await _repositoryProducto.GetAllByUserId(userId); //obtengo todos los productos con el idusuario que enviaron
                if (!Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var finalList = new List<ProductoVendido>();
                var alreadyUsedIList = new List<int>(); //creo lista de ids ya usadas para no dar mas vueltas de las necesarias
                foreach (var i in listProducts) //de todos los productos del mismo idusuario
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
                return Utils.Utils.ListCorrectResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, finalList, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}