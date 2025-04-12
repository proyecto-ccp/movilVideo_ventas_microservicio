using Videos.Aplicacion.Dto;

namespace Videos.Aplicacion.Consultas
{
    public interface IConsultasVideo
    {
        public Task<VideoOut> ObtenerVideoPorId(Guid id);
        public Task<VideoListOut> ObtenerVideos();
        public Task<VideoListOut> ObtenerVideosPorCliente(Guid idCliente);
    }
}
