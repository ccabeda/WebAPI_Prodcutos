using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using WebApi_Proyecto_Final.UnitOfWork;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceVenta : IServiceVenta
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceVenta> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceVenta(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceVenta> logger, APIResponse apiResponse)
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
                var sale = await _unitOfWork.repositoryVenta.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(sale))
                {
                    _logger.LogError("El id " + id + "no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.OKResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var listSales = await _unitOfWork.repositoryVenta.GetAll();
                if (Utils.Utils.CheckIfLsitIsNull<Venta>(listSales))
                {
                    _logger.LogError("La lista de ventas esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<VentaDto, Venta>(_mapper, listSales, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Create(VentaCreateDto saleCreate)
        {
            try
            {
                var existUserId = await _unitOfWork.repositoryUsuario.GetById(saleCreate.IdUsuario);
                if (Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId))
                {
                    _logger.LogError("El idUsuario" + saleCreate.IdUsuario + "no se encuentra registrado.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var sale = _mapper.Map<Venta>(saleCreate);
                await _unitOfWork.repositoryVenta.Create(sale);
                await _unitOfWork.Save();
                _logger.LogError("!Venta creada con exito¡");
                return Utils.Utils.OKResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> Update(VentaUpdateDto saleUpdate)
        {
            try
            {
                var sale = await _unitOfWork.repositoryVenta.GetById(saleUpdate.Id);
                var existUserId = await _unitOfWork.repositoryUsuario.GetById(saleUpdate.IdUsuario);
                if (Utils.Utils.VerifyIfObjIsNull<Venta>(sale))
                {
                    _logger.LogError("La venta" + saleUpdate.Comentarios + "no se encuentra registrada.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId))
                {
                    _logger.LogError("El idUsuario" + saleUpdate.IdUsuario + "no se encuentra registrado.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                _mapper.Map(saleUpdate, sale);
                await _unitOfWork.repositoryVenta.Update(sale);
                await _unitOfWork.Save();
                _logger.LogInformation("!La venta de id " + saleUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.OKResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var sale = await _unitOfWork.repositoryVenta.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(sale))
                {
                    _logger.LogError("El id " + id + "no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                var listProductsSold = await _unitOfWork.repositoryproductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                             //que seria llamando al repository de productos vendidos en el service de venta
                if (!Utils.Utils.PreventDeletionIfRelatedSoldProdcutExist<Venta>(sale, listProductsSold, id))
                {
                    _logger.LogError("La Venta no se puede eliminar porque hay un Producto vendido que contiene como VentaId este venta.");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                await _unitOfWork.repositoryVenta.Delete(sale);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Venta eliminada con exito!");
                return Utils.Utils.OKResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> GetAllByUserId(int idUsuario)
        {
            try
            {
                var listSales = await _unitOfWork.repositoryVenta.GetAllByUserId(idUsuario);
                if (Utils.Utils.CheckIfLsitIsNull<Venta>(listSales))
                {
                    _logger.LogError("La lista de ventas esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                return Utils.Utils.ListOKResponse<VentaDto, Venta>(_mapper, listSales, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }

        public async Task<APIResponse> CreateByUserId(int userId, List<ProductoDtoParaVentas> products)
        {
            try
            {
                var existUserId = await _unitOfWork.repositoryUsuario.GetById(userId);
                if (Utils.Utils.VerifyIfObjIsNull(existUserId))
                {
                    _logger.LogError("El idUsuario " + userId + "no se encuentra registrado");
                    return Utils.Utils.BadRequestResponse(_apiResponse);
                }
                if (Utils.Utils.CheckIfLsitIsNull<ProductoDtoParaVentas>(products))
                {
                    _logger.LogError("La lista de productos esta vacia.");
                    Utils.Utils.BadRequestResponse(_apiResponse);
                }
                List<Producto> finalProducts = new List<Producto>();
                foreach (ProductoDtoParaVentas p in products) //verifico que todos los productos que se intentan vender tengan stock, si alguno no se ejecuta la venta
                {
                    var product = await _unitOfWork.repositoryProducto.GetById(p.Id);
                    if (product == null || product.Stock < p.Stock)
                    {
                        if (product == null)
                        {
                            _logger.LogError("Un producto de la lista enviada no existe. Verificar los Ids enviados");

                        }
                        else
                        {
                            _logger.LogError("La cantidad de stock de " + product.Descripciones + " es insuficiente.");
                        }
                        _apiResponse.IsExit = false;
                        _apiResponse.StatusCode = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                    product!.Stock -= p.Stock; //actualizo stock
                    await _unitOfWork.repositoryProducto.Update(product);
                    finalProducts.Add(product);
                }
                Venta sale = new Venta();
                List<string> names = finalProducts.Select(p => p.Descripciones).ToList();
                string comment = string.Join(" - ", names);
                sale.Comentarios = comment;
                sale.IdUsuario = userId;
                await _unitOfWork.repositoryVenta.Create(sale);
                await _unitOfWork.Save(); //aca guardo para obtener la id de venta
                int pointer = 0;
                foreach (Producto p in finalProducts) //creo los productos vendidos con el stock que fue vendido 
                {
                    ProductoVendido productSold = new ProductoVendido();
                    productSold.IdVenta = sale.Id;
                    productSold.IdProducto = p.Id;
                    productSold.Stock = finalProducts[pointer].Stock;
                    pointer++;
                    await _unitOfWork.repositoryproductoVendido.Create(productSold); //creo los productos vendidos
                }
                await _unitOfWork.Save(); //aca guardo todos los cambios, en vez de guardar en cada uno.
                return Utils.Utils.OKResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}