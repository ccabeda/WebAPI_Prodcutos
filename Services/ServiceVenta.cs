using AutoMapper;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using System.Net;
using WebApi_Proyecto_Final.DTOs.VentaDto;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services.IService;
using WebApi_Proyecto_Final.DTOs.ProductoDto;
using FluentValidation;
using WebApi_Proyecto_Final.DTOs.UsuarioDto;

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
        private readonly IValidator<VentaCreateDto> _validator;
        private readonly IValidator<VentaUpdateDto> _validatorUpdate;
        public ServiceVenta(IRepositoryVenta repository, IRepositoryUsuario repositoryUsuario, IRepositoryProductoVendido repositoryProductoVendido, IMapper mapper, 
            IRepositoryProducto repositoryProducto, ILogger<ServiceVenta> logger, APIResponse apiResponse, IValidator<VentaCreateDto> validator, IValidator<VentaUpdateDto> validatorUpdate)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryProductoVendido = repositoryProductoVendido;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = apiResponse;
            _repositoryProducto = repositoryProducto;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var sale = await _repository.GetById(id); //busco en la db con la id
                if (sale == null)
                {
                    _logger.LogError("Error, el id ingresado no se encuentra registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<VentaDto>(sale);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la Venta: " + ex.Message);
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
                var listSales = await _repository.GetAll(); //traigo la lista de usuarios
                if (listSales == null)
                {
                    _logger.LogError("No hay ningúna venta registrada actualmente. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<VentaDto>>(listSales);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Ventas: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Create(VentaCreateDto saleCreate)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(saleCreate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existUserId = await _repositoryUsuario.GetById(saleCreate.IdUsuario);
                if (existUserId == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                var sale = _mapper.Map<Venta>(saleCreate);
                await _repository.Create(sale!);
                _logger.LogError("!Venta creada con exito¡");
                _apiResponse.Result = _mapper.Map<VentaDto>(sale);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear la Venta: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> Update(VentaUpdateDto saleUpdate)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(saleUpdate); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Exeption = errors;
                    return _apiResponse;
                }
                var existSale = await _repository.GetById(saleUpdate.Id); //verifico que el id ingresado este registrado en la db
                if (existSale == null)
                {
                    _logger.LogError("Error, la venta que intenta modificar no existe.");
                    _logger.LogError("Por favor, verifique que el id ingresado exista.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var existUserId = await _repositoryUsuario.GetById(saleUpdate.IdUsuario);
                if (existUserId == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    return _apiResponse;
                }
                _mapper.Map(saleUpdate, existSale);
                await _repository.Update(existSale); //guardo cambios
                _logger.LogInformation("!La venta de id " + saleUpdate.Id + " fue actualizado con exito!");
                _apiResponse.Result = _mapper.Map<VentaDto>(existSale);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar la Venta: " + ex.Message);
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
                var sale = await _repository.GetById(id);
                if (sale == null)
                {
                    _logger.LogError("El id ingresado no esta registrado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                var listProductsSold = await _repositoryProductoVendido.GetAll(); //verificacion para no utilizar borrado de cascada, es una alternativa,
                                                                                   //que seria llamando al repository de productos vendidos en el service de venta
                foreach (var i in listProductsSold)
                {
                    if (i.IdVenta == id)
                    {
                        _logger.LogError("Error. El producto vendido de id " + i.Id + " tiene como VentaId a esta venta.");
                        _logger.LogError("Modifica o elimina el producto vendido para eliminar esta venta.");
                        _apiResponse.IsExit = false;
                        _apiResponse.StatusCode = HttpStatusCode.Conflict;
                        return _apiResponse;
                    }
                }
                await _repository.Delete(sale);
                _logger.LogInformation("¡Venta eliminada con exito!");
                _apiResponse.Result = _mapper.Map<VentaDto>(sale);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar la Venta: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> GetAllByUserId(int idUsuario)
        {
            try
            {
                var listSales = await _repository.GetAllByUserId(idUsuario); //traigo la lista de usuarios
                if (listSales.Count == 0)
                {
                    _logger.LogError("No hay ningúna venta registrada a ese IdUsuario. Vuelve a intentarlo mas tarde.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<IEnumerable<VentaDto>>(listSales);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar obtener la lista de Ventas de ese IdUsuario: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }

        public async Task<APIResponse> CreateByUserId(int userId, List<ProductoDtoParaVentas> products)
        {
            try
            {
                var existUserId = await _repositoryUsuario.GetById(userId);
                if (existUserId == null)
                {
                    _logger.LogError("No existe usuario con el idUsuario enviado.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                if (products.Count == 0)
                {
                    _logger.LogError("La lista de productos para vender esta vacia.");
                    _apiResponse.IsExit = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
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
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<VentaDto>(sale);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear la Venta: " + ex.Message);
                _apiResponse.IsExit = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Exeption = new List<string> { ex.ToString() };
                return _apiResponse;
            }
        }
    }
}