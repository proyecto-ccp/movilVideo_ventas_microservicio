using Microsoft.AspNetCore.Http;

namespace Videos.Aplicacion.Dto
{
    public class VideoIn
    {
        //public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public IFormFile Video { get; set; }
        //public string Ruta { get; set; }
        //public string UrlVideo { get; set; }
        //public string UrlImagen { get; set; }
        //public string EstadoCarga { get; set; }
    }
}
