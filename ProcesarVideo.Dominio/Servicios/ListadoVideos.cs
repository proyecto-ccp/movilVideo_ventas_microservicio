using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;

namespace Videos.Dominio.Servicios
{
    public class ListadoVideos(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio= videoRepositorio;
        public async Task<List<Video>> ObtenerListado()
        {
            return await _videoRepositorio.ObtenerListado();
        }
    }
}
