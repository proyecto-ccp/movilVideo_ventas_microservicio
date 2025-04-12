using Videos.Dominio.Entidades;

namespace Videos.Dominio.Puertos.Repositorios
{
    public interface IVideoRepositorio
    {
        Task Cargar(Video video);
        Task<Video> ObtenerVideoPorId(Guid id);
        Task<List<Video>> ObtenerListado();
        Task<List<Video>> ObtenerVideosPorCliente(Guid clienteId);
    }
}
