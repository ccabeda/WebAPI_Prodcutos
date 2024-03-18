using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;

namespace WebApi_Proyecto_Final.Services.Utils
{
    public static class Utils
    {
        public static ActionResult<APIResponse> ControllerHelper(APIResponse apiresponse) 
        {
            switch (apiresponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(apiresponse);
                case HttpStatusCode.Created:
                    return new OkObjectResult(apiresponse);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(apiresponse);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(apiresponse);
                case HttpStatusCode.InternalServerError:
                    return new NotFoundObjectResult(apiresponse);
                default:
                    return new NotFoundObjectResult(apiresponse);
            }
        }

        public static APIResponse ErrorHandling(Exception ex, APIResponse apiresponse, ILogger logger) 
        {
            logger.LogError("Ocurrio un error inesperado. Error: " + ex.Message);
            apiresponse.IsExit = false;
            apiresponse.StatusCode = HttpStatusCode.InternalServerError;
            apiresponse.Exeption = new List<string> { ex.ToString() }; //lista para mantener el error
            return apiresponse;
        }

        public static bool VerifyIfObjIsNull<T>(T obj, APIResponse apiresponse, ILogger logger) //quiere que exista
        {
            if (obj == null) 
            {
                logger.LogError("Error con los datos ingresados, verifique que todo sea correcto.");
                apiresponse.IsExit = false;
                apiresponse.StatusCode = HttpStatusCode.BadRequest;
                return false;
            }
            return true;
        }

        public static bool CheckIfLsitIsNull<T>(List<T> model, APIResponse apiresponse, ILogger logger) 
        {
            if (model.Count == 0) //verifica (list == null)
            {
                apiresponse.IsExit = false;
                apiresponse.StatusCode = HttpStatusCode.BadRequest;
                logger.LogError("La lista deseada esta vacia.");
                return false;
            }
            return true;
        }

        public static APIResponse CorrectResponse<T,A>(IMapper mapper, A obj, APIResponse apiresponse) 
        {
            apiresponse.Result = mapper.Map<T>(obj);
            apiresponse.StatusCode = HttpStatusCode.OK;
            return apiresponse;
        }

        public static APIResponse ListCorrectResponse<T, A>(IMapper mapper, List<A> objs, APIResponse apiresponse)
        {
            apiresponse.Result = mapper.Map<IEnumerable<T>>(objs);
            apiresponse.StatusCode = HttpStatusCode.OK;
            return apiresponse;
        }

        public static bool CheckIfObjectExist<T>(T model, APIResponse apiresponse, ILogger logger)  //quiere que no exista
        {
            if (model != null)
            {
                apiresponse.IsExit = false;
                apiresponse.StatusCode = HttpStatusCode.Conflict;
                if (model is not Usuario)
                {
                    logger.LogError("El nombre ya se encuentra registrado. Por favor, utiliza otro.");
                }
                return false;
            }
            return true;
        }

        public static bool CheckIfNameAlreadyExist<T>(T modelOne, dynamic modelTwo, APIResponse apiresponse, ILogger logger) //funcion para verificar si ya existe el nombre en los updates (no usar anterior porque si
                                                                                                                             //no cambias el nombre no funciona)
        {
            if (modelOne != modelTwo)
            {
                if (modelOne != null)
                {
                    apiresponse.IsExit = false;
                    apiresponse.StatusCode = HttpStatusCode.Conflict;
                    if (modelOne is Usuario)
                    {
                        logger.LogError("El nombre de usuario o mail ya se encuentra registrado. Por favor, utiliza otro.");
                    }
                    else
                    {
                        logger.LogError("El nombre ya se encuentra registrado. Por favor, utiliza otro.");
                    }
                    return false;
                }
                return true;
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedSoldProdcutExist<T>(T obj,IEnumerable<ProductoVendido> list, APIResponse apiresponse, int id)
        {
            foreach (var i in list)
            {
                if (i.IdProducto == id && obj is Producto || i.IdVenta == id && obj is Venta)
                {
                    apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    apiresponse.IsExit = false;
                    return false;
                }
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedProductExist(IEnumerable<Producto> list, APIResponse apiresponse, int id)
        {
            foreach (var i in list)
            {
                if (i.IdUsuario == id)
                {
                    apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    apiresponse.IsExit = false;
                    return false;
                }
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedSalesExist(IEnumerable<Venta> list, APIResponse apiresponse, int id)
        {
            foreach (var i in list)
            {
                if (i.IdUsuario == id)
                {
                    apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    apiresponse.IsExit = false;
                    return false;
                }
            }
            return true;
        }

        public static bool VerifyPassword(string a, string b, ILogger logger, APIResponse apiresponse)
        {
            if (a != b)
            {
                logger.LogError("Error,contraseña incorrecta.");
                apiresponse.IsExit = false;
                apiresponse.StatusCode = HttpStatusCode.BadRequest;
                return false;
            }
            return true;
        }

    }
}
