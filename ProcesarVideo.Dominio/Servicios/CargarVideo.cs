using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;

namespace Videos.Dominio.Servicios
{
    public class CargarVideo(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio = videoRepositorio;
        public async Task Cargar(Video video)
        {
            if (ValidarVideo(video))
            {
                video.Id = Guid.NewGuid();
                video.FechaCreacion = DateTime.Now;
                await videoRepositorio.Cargar(video);


            }
            else
            {
                throw new InvalidOperationException("Valores incorrectos para los parametros minimos");
            }
        }

        public bool ValidarVideo(Video video)
        {
            return video.IdCliente != Guid.Empty && video.IdProducto != 0 && !string.IsNullOrEmpty(video.Nombre) && !string.IsNullOrEmpty(video.UrlVideo) && !string.IsNullOrEmpty(video.Archivo);
        }
    }
}
