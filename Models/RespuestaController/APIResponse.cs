using System.Net;

namespace WebApi_Proyecto_Final.Models.APIResponse
{
    public class APIResponse //clase para encapsular las respuestas de los endpoints y que sean iguales
    {
        public HttpStatusCode EstadoRespuesta { get; set; } //estado para los controllers
        public bool FueExitoso { get; set; } = true; //verifica si fue exitoso o ocurrio un error en el endpoint
        public List<String>? Exepciones { get; set; } //lista para guardar los mensajes de error
        public object? Resultado { get; set; } //almacena el objeto a devolver en los endpoints
    }
}

