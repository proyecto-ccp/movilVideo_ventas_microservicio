using Microsoft.AspNetCore.Http;

namespace Videos.Aplicacion.Dto
{
    public class VideoIn
    {
        public Guid IdCliente { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Video { get; set; }
    }
}
