using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.DTOs.ProductoVendidoDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.UnitOfWork;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceProductoVendido : IServiceProductoVendido
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceProductoVendido> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceProductoVendido(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceProductoVendido> logger, APIResponse apiResponse)
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
                var productSold = await _unitOfWork.repositoryproductoVendido.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(productSold))
                {
                    _logger.LogError("El producto vendido de id " + id + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var listProductsSold = await _unitOfWork.repositoryproductoVendido.GetAll();
                if (Utils.Utils.CheckIfLsitIsNull<ProductoVendido>(listProductsSold))
                {
                    _logger.LogError("La lista de Productos vendidos esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, listProductsSold, _apiResponse);
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
                var existProductId = await _unitOfWork.repositoryProducto.GetById(productSoldCreate.IdProducto);
                var existSaleId = await _unitOfWork.repositoryVenta.GetById(productSoldCreate.IdVenta);
                if (Utils.Utils.VerifyIfObjIsNull<Producto>(existProductId))
                {
                    _logger.LogError("El idProducto " + productSoldCreate.IdProducto + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Venta>(existSaleId))
                {
                    _logger.LogError("El idProducto " + productSoldCreate.IdVenta + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var productSold = _mapper.Map<ProductoVendido>(productSoldCreate);
                await _unitOfWork.repositoryproductoVendido.Create(productSold);
                await _unitOfWork.Save();
                _logger.LogInformation("!ProductoVendido creado con exito¡");
                return Utils.Utils.OKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var productSold = await _unitOfWork.repositoryproductoVendido.GetById(productSoldUpdate.Id);
                var existProductId = await _unitOfWork.repositoryProducto.GetById(productSoldUpdate.IdProducto);
                var existSaleId = await _unitOfWork.repositoryVenta.GetById(productSoldUpdate.IdVenta);
                if (Utils.Utils.VerifyIfObjIsNull<ProductoVendido>(productSold))
                {
                    _logger.LogError("El producto vendido enviado no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Producto>(existProductId))
                {
                    _logger.LogError("El idProducto " + productSoldUpdate.IdProducto + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Venta>(existSaleId))
                {
                    _logger.LogError("El idVenta " + productSoldUpdate.IdVenta + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                _mapper.Map(productSoldUpdate, productSold);
                await _unitOfWork.repositoryproductoVendido.Update(productSold);
                await _unitOfWork.Save();
                _logger.LogInformation("!El producto vendido de id " + productSoldUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.OKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var productSold = await _unitOfWork.repositoryproductoVendido.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(productSold))
                {
                    _logger.LogError("El id " + id + " no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                await _unitOfWork.repositoryproductoVendido.Delete(productSold);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Producto vendido eliminado con exito!");
                return Utils.Utils.OKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, productSold, _apiResponse);
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
                var listProducts = await _unitOfWork.repositoryProducto.GetAllByUserId(userId); //obtengo todos los productos con el idusuario que enviaron
                if (Utils.Utils.CheckIfLsitIsNull<Producto>(listProducts))
                {
                    _logger.LogError("La lista de Productos vendidos esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var finalList = new List<ProductoVendido>();
                var alreadyUsedIList = new List<int>(); //creo lista de ids ya usadas para no dar mas vueltas de las necesarias
                foreach (var i in listProducts) //de todos los productos del mismo idusuario
                {
                    if (!alreadyUsedIList.Contains(i.Id)) //si la id aun no se reviso
                    {
                        var resultado = await _unitOfWork.repositoryproductoVendido.GetByProductId(i.Id); //busco y me quedo con los que allan sido vendidos
                        if (resultado != null)
                        {
                            finalList.AddRange(resultado); //agrego la lista a una nueva lista
                        }
                    }
                    alreadyUsedIList.Add(i.Id); //agrego el Id ya usado para que no vuelva a usarse
                }
                return Utils.Utils.ListOKResponse<ProductoVendidoDTO, ProductoVendido>(_mapper, finalList, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}