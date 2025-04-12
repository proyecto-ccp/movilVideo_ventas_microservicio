using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;

namespace Videos.Dominio.Servicios
{
    public class ObtenerVideoPorCliente(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio = videoRepositorio;
        public async Task<List<Video>> ObtenerVideosPorCliente(Guid clienteId)
        {
            var videos = await _videoRepositorio.ObtenerVideosPorCliente(clienteId);
            if (videos == null || videos.Count == 0)
            {
                throw new Exception("No se encontraron videos para el cliente especificado");
            }
            return videos;
        }
    }
}
