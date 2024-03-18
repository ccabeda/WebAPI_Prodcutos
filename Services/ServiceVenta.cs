using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.DTOs.ProductoDto;

namespace WebApi_Proyecto_Final.Services
{
    public class ServiceVenta : IServiceVenta
    {
        private readonly IRepositoryVenta _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryProductoVendido _repositoryProductoVendido;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceVenta> _logger;
        private readonly APIResponse _apiResponse;
        public ServiceVenta(IRepositoryVenta repository, IRepositoryUsuario repositoryUsuario, IRepositoryProductoVendido repositoryProductoVendido, IMapper mapper, 
            IRepositoryProducto repositoryProducto, ILogger<ServiceVenta> logger, APIResponse apiResponse)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
            _repositoryProducto = repositoryProducto;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var sale = await _repository.GetById(id); //busco en la db con la id
                if (!Utils.Utils.VerifyIfObjIsNull(sale, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.CorrectResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var listSales = await _repository.GetAll(); //traigo la lista de usuarios
                if (!Utils.Utils.CheckIfLsitIsNull<Venta>(listSales, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<VentaDto, Venta>(_mapper, listSales, _apiResponse);
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
                var existUserId = await _repositoryUsuario.GetById(saleCreate.IdUsuario);
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var sale = _mapper.Map<Venta>(saleCreate);
                await _repository.Create(sale!);
                _logger.LogError("!Venta creada con exito¡");
                return Utils.Utils.CorrectResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var sale = await _repository.GetById(saleUpdate.Id); //verifico que el id ingresado este registrado en la db
                var existUserId = await _repositoryUsuario.GetById(saleUpdate.IdUsuario);
                if (!Utils.Utils.VerifyIfObjIsNull<Venta>(sale, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.VerifyIfObjIsNull<Usuario>(existUserId, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                _mapper.Map(saleUpdate, sale);
                await _repository.Update(sale); //guardo cambios
                _logger.LogInformation("!La venta de id " + saleUpdate.Id + " fue actualizado con exito!");
                return Utils.Utils.CorrectResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var sale = await _repository.GetById(id);
                if (Utils.Utils.VerifyIfObjIsNull(sale, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                var listProductsSold = await _repositoryProductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                   //que seria llamando al repository de productos vendidos en el service de venta
                if(!Utils.Utils.PreventDeletionIfRelatedSoldProdcutExist<Venta>(sale, listProductsSold, _apiResponse, id))
                {
                    _logger.LogError("La Venta no se puede eliminar porque hay un Producto vendido que contiene como VentaId este venta.");
                    return _apiResponse;
                }
                await _repository.Delete(sale);
                _logger.LogInformation("¡Venta eliminada con exito!");
                return Utils.Utils.CorrectResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
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
                var listSales = await _repository.GetAllByUserId(idUsuario); //traigo la lista de usuarios
                if (!Utils.Utils.CheckIfLsitIsNull<Venta>(listSales, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                return Utils.Utils.ListCorrectResponse<VentaDto, Venta>(_mapper, listSales, _apiResponse);
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
                var existUserId = await _repositoryUsuario.GetById(userId);
                if (Utils.Utils.VerifyIfObjIsNull(existUserId, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                if (!Utils.Utils.CheckIfLsitIsNull<ProductoDtoParaVentas>(products, _apiResponse, _logger))
                {
                    return _apiResponse;
                }
                List<Producto> finalProducts = new List<Producto>();
                foreach (ProductoDtoParaVentas p in products) //verifico que todos los productos que se intentan vender tengan stock, si alguno no se ejecuta la venta
                {
                    var product = await _repositoryProducto.GetById(p.Id);
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
                    await _repositoryProducto.Update(product);
                    finalProducts.Add(product);
                }
                Venta sale = new Venta();
                List<string> names = finalProducts.Select(p => p.Descripciones).ToList();
                string comment = string.Join(" - ", names);
                sale.Comentarios = comment;
                sale.IdUsuario = userId;
                await _repository.Create(sale); //creo la venta
                int pointer = 0;
                foreach (Producto p in finalProducts) //creo los productos vendidos con el stock que fue vendido 
                {
                    ProductoVendido productSold = new ProductoVendido();
                    productSold.IdVenta = sale.Id;
                    productSold.IdProducto = p.Id;
                    productSold.Stock = products[pointer].Stock;
                    pointer++;
                    await _repositoryProductoVendido.Create(productSold); //creo los productos vendidos
                }
                return Utils.Utils.CorrectResponse<VentaDto, Venta>(_mapper, sale, _apiResponse);
            }
            catch (Exception ex)
            {
                return Utils.Utils.ErrorHandling(ex, _apiResponse, _logger);
            }
        }
    }
}