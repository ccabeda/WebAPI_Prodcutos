using AutoMapper;
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

        public static bool VerifyIfObjIsNull<T>(T obj) //quiere que exista
        {
            if (obj == null) 
            {
                return true;
            }
            return false;
        }

        public static bool CheckIfLsitIsNull<T>(List<T> model) 
        {
            if (model.Count == 0) //verifica (list == null)
            {
                return true;
            }
            return false;
        }

        public static bool CheckIfNameAlreadyExist<T>(T modelOne, dynamic modelTwo) //funcion para verificar si ya existe el nombre en los updates (no usar anterior porque si
                                                                                                                             //no cambias el nombre no funciona)
        {
            if (modelOne != modelTwo)
            {
                if (modelOne != null)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool PreventDeletionIfRelatedSoldProdcutExist<T>(T obj,IEnumerable<ProductoVendido> list, int id)
        {
            foreach (var i in list)
            {
                if (i.IdProducto == id && obj is Producto || i.IdVenta == id && obj is Venta)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedProductExist(IEnumerable<Producto> list, int id)
        {
            foreach (var i in list)
            {
                if (i.IdUsuario == id)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedSalesExist(IEnumerable<Venta> list, int id)
        {
            foreach (var i in list)
            {
                if (i.IdUsuario == id)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool VerifyPassword(string a, string b)
        {
            if (a != b)
            {
                return false;
            }
            return true;
        }

        public static APIResponse BadRequestResponse(APIResponse apiresponse)
        {
            apiresponse.IsExit = false;
            apiresponse.StatusCode = HttpStatusCode.BadRequest;
            return apiresponse;
        }

        public static APIResponse ConflictResponse(APIResponse apiresponse)
        {
            apiresponse.IsExit = false;
            apiresponse.StatusCode = HttpStatusCode.Conflict;
            return apiresponse;
        }

        public static APIResponse OKResponse<T, A>(IMapper mapper, A obj, APIResponse apiresponse) //funcion para repsonder correctamente los getbyAlgo
        {
            apiresponse.Result = mapper.Map<T>(obj);
            apiresponse.StatusCode = HttpStatusCode.OK;
            return apiresponse;
        }

        public static APIResponse ListOKResponse<T, A>(IMapper mapper, List<A> objs, APIResponse apiresponse) //funcion para responder correctamente los getAll
        {
            apiresponse.Result = mapper.Map<IEnumerable<T>>(objs);
            apiresponse.StatusCode = HttpStatusCode.OK;
            return apiresponse;
        }
    }
}
