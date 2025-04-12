using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;

namespace Videos.Dominio.Servicios
{
    public class ObtenerVideo(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio = videoRepositorio;

        public async Task<Video> ObtenerVideoPorId(Guid id)
        {
            var video = await _videoRepositorio.ObtenerVideoPorId(id);

            if (video == null)
            {
                throw new Exception("El video no existe");
            }

            return video;
        }
    }
}
