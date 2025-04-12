using AutoMapper;
using System.Net;
using Videos.Aplicacion.Dto;
using Videos.Aplicacion.Enum;
using Videos.Dominio.Puertos.Repositorios;
using Videos.Dominio.Servicios;

namespace Videos.Aplicacion.Consultas
{
    public class ManejadorConsultas : IConsultasVideo
    {
        private readonly ObtenerVideo _obtenerVideo;
        private readonly ListadoVideos _listadoVideos;
        private readonly ObtenerVideoPorCliente _obtenerVideoPorCliente;
        private readonly IMapper _mapper;

        public ManejadorConsultas(IVideoRepositorio videoRepositorio, IMapper mapper)
        {
            _obtenerVideo = new ObtenerVideo(videoRepositorio);
            _listadoVideos = new ListadoVideos(videoRepositorio);
            _obtenerVideoPorCliente = new ObtenerVideoPorCliente(videoRepositorio);

            _mapper = mapper;
        }

        public async Task<VideoOut> ObtenerVideoPorId(Guid id)
        {
            VideoOut videoOut = new();
            try 
            {
                var video = await _obtenerVideo.ObtenerVideoPorId(id);

                if (video == null || video.Id == Guid.Empty)
                {
                    videoOut.Resultado = Resultado.SinRegistros;
                    videoOut.Mensaje = "Video NO encontrado";
                    videoOut.Status = HttpStatusCode.NoContent;
                }
                else
                {
                    videoOut.Resultado = Resultado.Exitoso;
                    videoOut.Mensaje = "Video encontrado";
                    videoOut.Status = HttpStatusCode.OK;
                    videoOut.Video = _mapper.Map<VideoDto>(video);
                }
            }
            catch (Exception ex)
            {
                videoOut.Resultado = Resultado.Error;
                videoOut.Mensaje = ex.Message;
                videoOut.Status = HttpStatusCode.InternalServerError;
            }

            return videoOut;
        }

        public async Task<VideoListOut> ObtenerVideos()
        {
            VideoListOut output = new()
            { 
                Videos = []
            };

            try {
                var videos = await _listadoVideos.ObtenerListado();

                if (videos == null || videos.Count == 0)
                {
                    output.Resultado = Resultado.SinRegistros;
                    output.Mensaje = "No se encontraron videos";
                    output.Status = HttpStatusCode.NoContent;
                }
                else
                {
                    output.Resultado = Resultado.Exitoso;
                    output.Mensaje = "Videos encontrados";
                    output.Status = HttpStatusCode.OK;
                    output.Videos = _mapper.Map<List<VideoDto>>(videos);
                }
            }
            catch (Exception ex)
            {
                output.Resultado = Resultado.Error;
                output.Mensaje = ex.Message;
                output.Status = HttpStatusCode.InternalServerError;
            }

            return output;
        }

        public async Task<VideoListOut> ObtenerVideosPorCliente(Guid idCliente)
        {
            VideoListOut output = new()
            {
                Videos = []
            };

            try
            {
                var videos = await _obtenerVideoPorCliente.ObtenerVideosPorCliente(idCliente);

                if (videos == null || videos.Count == 0)
                {
                    output.Resultado = Resultado.SinRegistros;
                    output.Mensaje = "No se encontraron videos";
                    output.Status = HttpStatusCode.NoContent;
                }
                else
                {
                    output.Resultado = Resultado.Exitoso;
                    output.Mensaje = "Videos encontrados";
                    output.Status = HttpStatusCode.OK;
                    output.Videos = _mapper.Map<List<VideoDto>>(videos);
                }
            }
            catch (Exception ex)
            {
                output.Resultado = Resultado.Error;
                output.Mensaje = ex.Message;
                output.Status = HttpStatusCode.InternalServerError;
            }

            return output;
        }
    }
}
